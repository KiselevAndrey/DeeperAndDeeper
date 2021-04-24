using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float dampTime = 0.2f;

    Ball _ball;
    Vector3 _moveVelocity;
    Vector3 _desiredPosition;
    float _minY;
    bool _moving;

    void Start()
    {
        _ball = Ball.singleton;
        transform.position = _ball.transform.position;
        _minY = _ball.transform.position.y;
    }

    void FixedUpdate()
    {
        if(_minY > _ball.transform.position.y)
        {
            _minY = _ball.transform.position.y;
            _desiredPosition = _ball.transform.position;
            _moving = true;
        }

        if (_moving) Move();
    }

    void Move()
    {
        transform.position = Vector3.SmoothDamp(transform.position, _desiredPosition, ref _moveVelocity, dampTime);

        if (transform.position.y == _minY) _moving = false;
    }


}
