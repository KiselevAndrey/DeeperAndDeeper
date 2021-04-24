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

    int _currentHealth;

    public static Action PlayerDie;

    #region Awake OnDestroy Start Update
    void Awake()
    {
        singleton = this;
        PlatformManager.BallHit += Hit;
    }
    private void OnDestroy()
    {
        PlatformManager.BallHit -= Hit;
    }

    private void Start()
    {
        _currentHealth = maxHealth;
        healthText.text = _currentHealth.ToString();
        SetScale();
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
            print("maxspeed " + rb.velocity.magnitude);
            SetSpeed(maxSpeed);
        }
    }

    void CheckLowSpeed()
    {
        if (rb.velocity.magnitude < 0.1f)
        {
            print("lowspeed " + rb.velocity.magnitude);
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


    private void OnCollisionEnter(Collision collision)
    {
        SetSpeed(maxSpeed);
    }
}
