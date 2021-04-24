using UnityEngine;

public class Ball : MonoBehaviour
{
    static public Ball singleton;

    [SerializeField] Rigidbody rb;
    [SerializeField] float maxSpeed;

    [Header("Health")]
    [SerializeField] int maxHealth;
    [SerializeField] float startScale = 0.3f;
    [SerializeField] float minScale = 0.1f;

    int _currentHealth;

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
        SetScale();
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude > maxSpeed) rb.velocity = rb.velocity.normalized * maxSpeed;        
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space)) Time.timeScale = Time.timeScale > 0 ? 0f : 1f;
    }
    #endregion

    #region Hit
    void SetScale()
    {
        float percentHealth = (float)_currentHealth / maxHealth;
        transform.localScale = Vector3.one * (minScale + (startScale - minScale) * percentHealth);
    }

    void Hit(int damage)
    {
        if (damage <= 0) return;

        _currentHealth -= damage;
        if (_currentHealth < 0)
        {
            _currentHealth = 0;
        }
        SetScale();
    }
    #endregion  

    private void OnCollisionEnter(Collision collision)
    {
    }
}
