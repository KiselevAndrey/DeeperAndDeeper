using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    static public Ball singleton;

    [SerializeField] Rigidbody rb;
    [SerializeField] SphereCollider sphereCollider;
    [SerializeField] float maxSpeed;

    [Header("Health")]
    public int maxHealth;
    [SerializeField] float startScale = 0.3f;
    [SerializeField] float minScale = 0.1f;
    [SerializeField] Text healthText;

    [Header("Money")]
    [SerializeField] GameObject moneyPrefab;
    [SerializeField] Transform whereToFly;
    public float goalMultiplier = 1;
    [SerializeField] Text moneyText;

    [HideInInspector] public float money;
    [HideInInspector] public int healthBonusPurchased;
    [HideInInspector] public int goldAddedBonusPurchased;
    [HideInInspector] public int punchingBonusPurchased;
    [HideInInspector] public float currentScore;
    [HideInInspector] public int bestScore;

    int _currentHealth;
    int _goalsWithoutHit;
    bool _notPlaying;
    int _punchingCount;

    public static Action PlayerDie;

    #region Awake OnDestroy Update
    void Awake()
    {
        singleton = this;
        //PlatformManager.BallHit += Hit;
        //PlatformManager.Goal += Goal;
    }
    private void OnDestroy()
    {
        //PlatformManager.BallHit -= Hit;
        //PlatformManager.Goal -= Goal;
    }

    private void FixedUpdate()
    {
        if (_notPlaying) return;

        //rb.velocity = Vector3.down * maxSpeed;
        CheckMaxSpeed();
        //CheckLowSpeed();
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && !_notPlaying) Time.timeScale = Time.timeScale > 0 ? 0f : 1f;
    }
    #endregion

    #region Moving
    void CheckMaxSpeed()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            SetSpeed(maxSpeed);
        }
    }

    void CheckLowSpeed()
    {
        if (rb.velocity.magnitude < 0.1f)
        {
            StartCoroutine(StartDown());
        }
    }

    IEnumerator StartDown()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        
        if (rb.velocity.magnitude < 0.1f)
            SetSpeed(-maxSpeed);
        print("minspeed");
    }

    void SetSpeed(float speed) => rb.velocity = rb.velocity.normalized * speed;
    #endregion

    #region Hit
    void SetScale()
    {
        float percentHealth = (float)_currentHealth / maxHealth;
        transform.localScale = Vector3.one * Mathf.Lerp(minScale, startScale, percentHealth);// (minScale + (startScale - minScale) * percentHealth);
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
            Time.timeScale = 0f;
            bestScore = Mathf.Max(bestScore, (int)currentScore);
            PlayerDie();
        }
        SetScale();
    }
    #endregion

    #region Goal
    void Goal(Vector3 goalPos, int addedScore)
    {
        _goalsWithoutHit += addedScore;
        AddMoney(_goalsWithoutHit * goalMultiplier);
    }
    #endregion

    #region Money
    void AddMoney(float addedMoney)
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

    #region Save Load Reset
    public void Save() => SaveSystem.SaveBall(this);

    public void Load(bool notPlaying = false)
    {
        SaveSystem.LoadBall()?.LoadData(ref singleton);

        _notPlaying = notPlaying;
        if (notPlaying) return;

        _currentHealth = maxHealth;
        healthText.text = _currentHealth.ToString();
        SetScale();
        AddMoney(0);
        if (punchingBonusPurchased > 0) sphereCollider.isTrigger = true;
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

    #region OnCollision OnTrigger
    private void OnCollisionEnter(Collision collision)
    {
        SetSpeed(maxSpeed);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!sphereCollider.isTrigger) return;
        
        _punchingCount++;
        if (punchingBonusPurchased <= _punchingCount)
            sphereCollider.isTrigger = false;
    }
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
}
