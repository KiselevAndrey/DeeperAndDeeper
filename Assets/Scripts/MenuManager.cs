using System.Collections;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    Ball _ball;

    void Start()
    {
        _ball = gameObject.AddComponent<Ball>();
        _ball.Load(true);
    }

    public void ResetPlayer(bool delRecord) => _ball.ResetMe(delRecord);
}
