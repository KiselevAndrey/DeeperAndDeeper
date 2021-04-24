using UnityEngine;

public class Ball : MonoBehaviour
{
    static public Ball singleton;

    [SerializeField] Rigidbody rb;
    [SerializeField] float maxSpeed;


    // Start is called before the first frame update
    void Awake()
    {
        singleton = this;
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude > maxSpeed) rb.velocity = rb.velocity.normalized * maxSpeed;

    }

    private void OnCollisionEnter(Collision collision)
    {
    }
}
