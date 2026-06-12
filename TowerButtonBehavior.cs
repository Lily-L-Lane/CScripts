using UnityEngine;

public class TowerButtonBehavior : MonoBehavior
{
    public int towerIndex;
    int towerCost;
    Button button;
    void Start()
    {
        button = GetComponent<Button>();
        towerCost = TowerBuilder.Instance.towers[towerIndex].cost;
        towerCost = TowerBuilder.Instance.GetSelectedTowerCost(towerIndex);
    }
    void Update()
    {
        if (!MoneyManager.Instance)
        {
            return;
        }
        if(MoneyManager.Instance.GetCurrentMoney() > towerCost)
        {
            button.interactable = true;
        } else
        {
            button.interactable = false;
        }
    }
}