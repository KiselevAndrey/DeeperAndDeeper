using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager singleton;
    public enum States { Fall, Jump }

    public static Action CheckState;
    public static Action PlayerDie;

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
    private bool _notPlaying;
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
    private void CollisionTreatment(Vector3 collisionPos, int damage, PlatformManager.Type platformType)
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

            default:
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
        _currentHealth -= damage;
        healthText.text = _currentHealth.ToString();

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            StopGame();
            bestScore = Mathf.Max(bestScore, (int)currentScore);
            PlayerDie();
        }
        SetScale();
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
    private void ChangeState(States newState)
    {
        if (_currentState == newState) return;
        
        _currentState = newState;
        switch (newState)
        {
            case States.Fall:
                _animator.SetTrigger("Fall");
                break;

            case States.Jump:
                _animator.SetTrigger("Jump");
                break;

            default:
                break;
        }
    }
    #endregion

    #region Save Load Reset
    public void Save() => SaveSystem.SavePlayer(this);

    public void Load(bool notPlaying = false)
    {
        SaveSystem.LoadPlayer()?.LoadData(ref singleton);

        _notPlaying = notPlaying;
        if (notPlaying) return;

        _currentHealth = maxHealth;
        healthText.text = _currentHealth.ToString();
        SetScale();
        AddMoney(0);
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
}
