using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    static public Ball singleton;

    [SerializeField] Rigidbody rb;
    [SerializeField] float maxSpeed;

    [Header("Health")]
    [SerializeField] int maxHealth;
    [SerializeField] float startScale = 0.3f;
    [SerializeField] float minScale = 0.1f;
    [SerializeField] Text healthText;

    [Header("Money")]
    [SerializeField] GameObject moneyPrefab;
    [SerializeField] Transform whereToFly;
    public float GoalMultiplier = 1;
    [SerializeField] Text moneyText;

    int _currentHealth;
    int _goalsWithoutHit;
    float _money;

    public static Action PlayerDie;

    #region Awake OnDestroy Start Update
    void Awake()
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

    private void Start()
    {
        _currentHealth = maxHealth;
        healthText.text = _currentHealth.ToString();
        SetScale();
        AddMoney(0);
    }

    private void FixedUpdate()
    {
        CheckMaxSpeed();
        //CheckLowSpeed();
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space)) Time.timeScale = Time.timeScale > 0 ? 0f : 1f;
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
            PlayerDie();
        }
        SetScale();
    }
    #endregion

    #region Goal
    void Goal(Vector3 goalPos)
    {
        _goalsWithoutHit++;
        AddMoney(_goalsWithoutHit * GoalMultiplier);
    }
    #endregion

    #region Money
    void AddMoney(float addedMoney)
    {
        _money += addedMoney;
        moneyText.text = MoneyToString();
    }

    string MoneyToString()
    {
        string[] moneyEndings = { "", "K", "M", "B", "T", "q", "Q", "s", "S", "N", "d", "U", "D" };
        int period = 0;
        float money = (int)_money;
        while (money > 1000)
        {
            money /= 1000;
            period++;
        }
        return money.ToString() + " " + moneyEndings[period];
    }
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        SetSpeed(maxSpeed);
    }
}
