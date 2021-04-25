using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] Text moneyText;

    [Header("From ball")]
    [SerializeField] Text healthPrice;
    [SerializeField] Text goldMultipliedPrice;

    [Header("From column")]
    [SerializeField] Text rotationSpeedPrice;


    [Header("Additional options")]
    [SerializeField] GameObject noMoney;

    Ball _ball;
    ColumnRotate _columnRotate;
    int _noMoneyCount;

    void Start()
    {
        _ball = gameObject.AddComponent<Ball>(); 
        _columnRotate = gameObject.AddComponent<ColumnRotate>();

        _ball.Load(true);
        _columnRotate.Load();

        UpdateTexts();
    }

    #region UpdateTexts
    void UpdateTexts()
    {
        UpdateMoneyText();
        UpdateHealthText();
        UpdateGoldMultipliedText();
        UpdateRotationSpeedText();
    }

    void UpdateMoneyText() => moneyText.text = Ball.FloatToString(_ball.money);
    void UpdateHealthText() => healthPrice.text = Ball.FloatToString(CalculatePrice(_ball.healthBonusPurchased));
    void UpdateGoldMultipliedText() => goldMultipliedPrice.text = Ball.FloatToString(CalculatePrice(_ball.goldAddedBonusPurchased));
    void UpdateRotationSpeedText() => rotationSpeedPrice.text = Ball.FloatToString(CalculatePrice(_columnRotate.speedBonusPurchased));

    int CalculatePrice(int bonucPurchased) => (int)Mathf.Pow(2, bonucPurchased + 1);
    #endregion

    #region TryBuy
    public void TryBuyHealth()
    {
        int price = CalculatePrice(_ball.healthBonusPurchased);
        if (price > _ball.money)
        {
            StartCoroutine(NoMoney(++_noMoneyCount));
            return;
        }

        _ball.money -= price;
        _ball.BuyHealth();

        UpdateMoneyText();
        UpdateHealthText();
    }

    public void TryBuyGoldMultiplier()
    {
        int price = CalculatePrice(_ball.goldAddedBonusPurchased);
        if (price > _ball.money)
        {
            StartCoroutine(NoMoney(++_noMoneyCount));
            return;
        }

        _ball.money -= price;
        _ball.BuyGoldMultiplied();

        UpdateMoneyText();
        UpdateGoldMultipliedText();
    }
    
    public void TryBuyRotationSpeed()
    {
        int price = CalculatePrice(_columnRotate.speedBonusPurchased);
        if (price > _ball.money)
        {
            StartCoroutine(NoMoney(++_noMoneyCount));
            return;
        }

        _ball.money -= price;
        _columnRotate.BuyRotationSpeed();

        UpdateMoneyText();
        UpdateRotationSpeedText();
    }

    IEnumerator NoMoney(int count)
    {
        noMoney.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        if(_noMoneyCount == count)
            noMoney.SetActive(false);
    }
    #endregion

    #region ExitShop
    public void ExitShop()
    {
        _ball.Save();
        _columnRotate.Save();
    }
    #endregion
}