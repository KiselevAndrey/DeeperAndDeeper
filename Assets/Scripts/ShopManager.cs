using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("From ball")]
    [SerializeField] Text healthPrice;
    [SerializeField] Text goldMultipliedPrice;

    Ball _ball;
    ColumnRotate _columnRotate;

    void Start()
    {
        _ball.Load(false);
        _columnRotate.Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
