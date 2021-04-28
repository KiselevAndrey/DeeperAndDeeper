using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] Text moneyText;

    [Header("From ball")]
    [SerializeField] Text healthPrice;
    [SerializeField] Text goldMultipliedPrice;
    [SerializeField] Text punchingPrice;

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
        UpdateText(moneyText, Ball.FloatToString(_ball.money));
        UpdateText(healthPrice, Ball.FloatToString(CalculatePrice(_ball.healthBonusPurchased)));
        UpdateText(goldMultipliedPrice, Ball.FloatToString(CalculatePrice(_ball.goldAddedBonusPurchased)));
        UpdateText(punchingPrice, Ball.FloatToString(CalculatePrice(_ball.punchingBonusPurchased)));
    }

    void UpdateText(Text text, string str) => text.text = str;

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

        UpdateText(moneyText, Ball.FloatToString(_ball.money));
        UpdateText(healthPrice, Ball.FloatToString(CalculatePrice(_ball.healthBonusPurchased)));
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

        UpdateText(moneyText, Ball.FloatToString(_ball.money));
        UpdateText(goldMultipliedPrice, Ball.FloatToString(CalculatePrice(_ball.goldAddedBonusPurchased)));
    }
    
    public void TryBuyPunching()
    {
        int price = CalculatePrice(_ball.punchingBonusPurchased);
        if (price > _ball.money)
        {
            StartCoroutine(NoMoney(++_noMoneyCount));
            return;
        }

        _ball.money -= price;
        _ball.BuyPunching();

        UpdateText(moneyText, Ball.FloatToString(_ball.money));
        UpdateText(punchingPrice, Ball.FloatToString(CalculatePrice(_ball.punchingBonusPurchased)));
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