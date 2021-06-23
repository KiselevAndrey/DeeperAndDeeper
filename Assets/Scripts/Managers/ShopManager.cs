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

    PlayerManager _player;
    //ColumnRotate _columnRotate;
    int _noMoneyCount;

    void Start()
    {
        _player = gameObject.AddComponent<PlayerManager>(); 
        //_columnRotate = gameObject.AddComponent<ColumnRotate>();

        _player.Load(true);
        //_columnRotate.Load();

        UpdateTexts();
    }

    #region UpdateTexts
    void UpdateTexts()
    {
        UpdateText(moneyText, PlayerManager.FloatToString(_player.money));
        UpdateText(healthPrice, PlayerManager.FloatToString(CalculatePrice(_player.healthBonusPurchased)));
        UpdateText(goldMultipliedPrice, PlayerManager.FloatToString(CalculatePrice(_player.goldAddedBonusPurchased)));
        UpdateText(punchingPrice, PlayerManager.FloatToString(CalculatePrice(_player.punchingBonusPurchased)));
    }

    void UpdateText(Text text, string str) => text.text = str;

    int CalculatePrice(int bonucPurchased) => (int)Mathf.Pow(2, bonucPurchased + 1);
    #endregion

    #region TryBuy
    public void TryBuyHealth()
    {
        int price = CalculatePrice(_player.healthBonusPurchased);
        if (price > _player.money)
        {
            StartCoroutine(NoMoney(++_noMoneyCount));
            return;
        }

        _player.money -= price;
        _player.BuyHealth();

        UpdateText(moneyText, PlayerManager.FloatToString(_player.money));
        UpdateText(healthPrice, PlayerManager.FloatToString(CalculatePrice(_player.healthBonusPurchased)));
    }

    public void TryBuyGoldMultiplier()
    {
        int price = CalculatePrice(_player.goldAddedBonusPurchased);
        if (price > _player.money)
        {
            StartCoroutine(NoMoney(++_noMoneyCount));
            return;
        }

        _player.money -= price;
        _player.BuyGoldMultiplied();

        UpdateText(moneyText, PlayerManager.FloatToString(_player.money));
        UpdateText(goldMultipliedPrice, PlayerManager.FloatToString(CalculatePrice(_player.goldAddedBonusPurchased)));
    }
    
    public void TryBuyPunching()
    {
        int price = CalculatePrice(_player.punchingBonusPurchased);
        if (price > _player.money)
        {
            StartCoroutine(NoMoney(++_noMoneyCount));
            return;
        }

        _player.money -= price;
        _player.BuyPunching();

        UpdateText(moneyText, PlayerManager.FloatToString(_player.money));
        UpdateText(punchingPrice, PlayerManager.FloatToString(CalculatePrice(_player.punchingBonusPurchased)));
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
        _player.Save();
        //_columnRotate.Save();
    }
    #endregion
}