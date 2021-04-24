using UnityEngine;

public class Ball : MonoBehaviour
{
    static public Ball singleton;

    [SerializeField] Rigidbody rb;
    [SerializeField] float maxSpeed;

    [Header("Health")]
    [SerializeField] int maxLife;
    [SerializeField] float startScale = 0.3f;
    [SerializeField] float minScale = 0.1f;

    #region Awake Start Update
    void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude > maxSpeed) rb.velocity = rb.velocity.normalized * maxSpeed;

        if (Input.GetKeyUp(KeyCode.Space)) Time.timeScale = Time.timeScale > 0 ? 0 : 1;
    }
    #endregion

    #region Hit

    #endregion  

    private void OnCollisionEnter(Collision collision)
    {
    }
}
