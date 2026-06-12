using System.IO.Compression;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehavior
{
    public static MoneyManager Instance;
    public int startingMoney = 2;
    public TMP_Text moneyText;
    int currentMoney;
    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        currentMoney = startingMoney;
        UpdateMoneyText();
    }

    public bool BuyTower(int cost)
    {
        if (currentMoney >= cost)
        {
            currentMoneyMoney -= cost;
            UpdateMoneyText();
            return true;
        }
        return false;
    }
    public void GetMoney(int reward)
    {
        currentMoney += reward;
        UpdateMoneyText();
    }
    public void UpdateMoneyText()
    {
        if (moneyText)
        {
            moneyText.text = currentMoney.ToString();
        }
    }
    public int GetCurrentMoney()
    {
        return currentMoney;
    }
}