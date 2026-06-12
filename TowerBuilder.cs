using System.ComponentModel;
using System.Diagnostics;
using System.Security.Principal;
using UnityEngine;

public class TowerBuilder: MonoBehaviour
{
    [System.Serializable]
    public class Tower
    {
        public string name;
        public GameObject prefab;
        public int cost;
    }
    public Tower[] towers;
    int selectedTowerIndex;
    public static TowerBuilder Instance {get; private set;}
    bool selectedTower = false; 
    void Awake()
    {
        if(Instance != null & Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        //SelectTower(1);
    }

    public void SelectTower(int index)
    {
        if(index >= 0 && index < towers.Length)
        {
            selectedTowerIndex = index;
            selectedTower = true;
        }
        else
        {
            Debug.Log("Invalid Tower index.");
        }
        
    }
    public GameObject GetSelectedTowerPrefab()
    {
        return towers[selectedTowerIndex].prefab;
    }

    public int GetSelectedTowerCost()
    {
        return towers[selectedTowerIndex].cost;
    }

    public int GetSelectedTowerCost(int index)
    {
        return towers[index].cost;
    }

    public bool HasSelectedTower()
    {
        return selectedTower;
    }

    public void ClearSelection()
    {
        selectedTower = false;
        selectedTowerIndex = -100;
    }
}