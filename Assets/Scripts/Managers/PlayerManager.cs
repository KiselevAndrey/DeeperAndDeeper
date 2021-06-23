using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager singleton;
    public enum States { Fall, Jump, Punching }

    public static Action CheckState;
    public static Action PlayerDie;
    public static Action<Vector3> Punching;

    [Header("Main component")]
    [SerializeField] private Animator _animator;

    [Header("Health")]
    public int maxHealth;
    [SerializeField] private float startScale = 0.3f;
    [SerializeField] private float minScale = 0.1f;
    [SerializeField] private Text healthText;

    [Header("Money")]
    public float goalMultiplier = 1;
    [SerializeField] private Text moneyText;

    [HideInInspector] public float money;
    [HideInInspector] public int healthBonusPurchased;
    [HideInInspector] public int goldAddedBonusPurchased;
    [HideInInspector] public int punchingBonusPurchased;
    [HideInInspector] public float currentScore;
    [HideInInspector] public int bestScore;
   
    private int _currentHealth;
    private int _goalsWithoutHit;
    private int _punchingCount;
    private States _currentState;

    #region Awake OnDestroy
    private void Awake()
    {
        singleton = this;
        PlatformManager.Collision += CollisionTreatment;
    }
    private void OnDestroy()
    {
        PlatformManager.Collision -= CollisionTreatment;
    }
    #endregion

    #region Starts from animation
    private void CheckingState() => CheckState();
    #endregion

    #region Starts from event
    private void CollisionTreatment(int damage, PlatformManager.Type platformType)
    {

        if (_punchingCount > 0)
        {
            CollisionTreatmentPunching(damage, platformType);
        }
        else
        {
            CollisionTreatmentNormal(damage, platformType);
        }
    }

    private void CollisionTreatmentPunching(int damage, PlatformManager.Type platformType)
    {
        ChangeState(States.Punching, true);
        switch (platformType)
        {
            case PlatformManager.Type.Exit:
                _goalsWithoutHit += damage;
                AddMoney(_goalsWithoutHit * goalMultiplier);
                _punchingCount++;
                break;

            case PlatformManager.Type.Trap:
                Hit(damage / 2);
                break;

            case PlatformManager.Type.Normal:
                AddMoney(_goalsWithoutHit * goalMultiplier);
                break;

            case PlatformManager.Type.Start:
                AddMoney(_goalsWithoutHit * goalMultiplier);
                break;

            case PlatformManager.Type.BonusPunch:
                AddPunching(damage);
                break;

            case PlatformManager.Type.BonusLife:
                AddHealth(damage);
                break;
        }
    }

    private void CollisionTreatmentNormal(int damage, PlatformManager.Type platformType)
    {
        switch (platformType)
        {
            case PlatformManager.Type.Exit:
                ChangeState(States.Fall);
                _goalsWithoutHit += damage;
                AddMoney(_goalsWithoutHit * goalMultiplier);
                break;

            case PlatformManager.Type.Trap:
            case PlatformManager.Type.Normal:
                ChangeState(States.Jump);
                Hit(damage);
                break;

            case PlatformManager.Type.Start:
                ChangeState(States.Jump);
                break;

            case PlatformManager.Type.BonusPunch:
                ChangeState(States.Jump);
                AddPunching(damage);
                break;

            case PlatformManager.Type.BonusLife:
                ChangeState(States.Jump);
                AddHealth(damage);
                break;
        }
    }
    #endregion

    #region Hit
    void SetScale()
    {
        float percentHealth = (float)_currentHealth / maxHealth;
        transform.localScale = Vector3.one * Mathf.Lerp(minScale, startScale, percentHealth);
    }

    void Hit(int damage)
    {
        if (damage <= 0) return;

        _goalsWithoutHit = 0;
        AddHealth(-damage);

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            StopGame();
            bestScore = Mathf.Max(bestScore, (int)currentScore);
            PlayerDie();
        }
        SetScale();
    }

    private void AddHealth(int value)
    {
        _currentHealth += value;
        healthText.text = _currentHealth.ToString();
    }
    #endregion

    #region Money
    private void AddMoney(float addedMoney)
    {
        money += addedMoney;
        currentScore += addedMoney;
        moneyText.text = FloatToString(money);
    }

    public static string FloatToString(float score)
    {
        string[] endings = { "", "K", "M", "B", "T", "q", "Q", "s", "S", "N", "d", "U", "D" };
        int period = 0;
        float num = (int)score;
        while (num > 1000)
        {
            num /= 1000;
            period++;
        }
        return num.ToString() + " " + endings[period];
    }
    #endregion

    #region State
    private void ChangeState(States newState, bool changeAlways = false)
    {
        if (_currentState == newState && !changeAlways) return;
        
        switch (newState)
        {
            case States.Fall:
                _animator.SetTrigger("Fall");
                break;

            case States.Jump:
                _animator.SetTrigger("Jump");
                break;

            case States.Punching:
                if (_currentState != newState) _animator.SetTrigger("Punching");
                _punchingCount--;
                Punching(transform.position);
                break;
        }
        _currentState = newState;
    }
    #endregion

    #region Save Load Reset
    public void Save() => SaveSystem.SavePlayer(this);

    public void Load(bool notPlaying = false)
    {
        SaveSystem.LoadPlayer()?.LoadData(ref singleton);

        if (notPlaying) return;

        _currentHealth = maxHealth;
        healthText.text = _currentHealth.ToString();
        SetScale();
        AddMoney(0);
        ChangeState(States.Fall);
    }

    public void ResetMe(bool delBestScore)
    {
        maxHealth = 3;
        goalMultiplier = 1;
        money = 0;
        healthBonusPurchased = 0;
        goldAddedBonusPurchased = 0;
        punchingBonusPurchased = 0;
        if (delBestScore) bestScore = 0;

        Save();
    }
    #endregion

    #region Another Functions
    private void StopGame() => Time.timeScale = 0f;
    #endregion

    #region Shoping
    public void BuyHealth()
    {
        maxHealth++;
        healthBonusPurchased++;
    }
    public void BuyGoldMultiplied()
    {
        goalMultiplier++;
        goldAddedBonusPurchased++;
    }

    public void BuyPunching()
    {
        punchingBonusPurchased++;
    }
    #endregion

    #region Bonuses
    private void AddPunching(int value)
    {
        _punchingCount += value;
        ChangeState(States.Punching, true);
    }
    #endregion
}
