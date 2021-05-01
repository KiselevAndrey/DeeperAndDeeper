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
        PlatformManager.BallHit += Hit;
        PlatformManager.Goal += Goal;
    }
    private void OnDestroy()
    {
        PlatformManager.BallHit -= Hit;
        PlatformManager.Goal -= Goal;
    }
    #endregion

    #region Starts from animation
    private void CheckingState() => CheckState();
    #endregion

    #region Starts from event
    private void Goal(Vector3 goalPos, int addedScore)
    {
        _goalsWithoutHit += addedScore;
        AddMoney(_goalsWithoutHit * goalMultiplier);
    }

    private void Hit(int damage)
    {

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
    public void ChangeState(States newState)
    {
        if (_currentState == newState) return;

        _currentState = newState;
        switch (_currentState)
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
}
