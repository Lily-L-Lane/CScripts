using System.Diagnostics;
using UnityEngine;

public class TileBehavior : MonoBehaviour
{
    public Material highlightMaterial;
    public GameObject towerPrefab;
    Material originalMaterial;
    Renderer _renderer;
    bool tileTower;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        originalMaterial = _renderer.sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        if (!TowerBuilder.Instance.HasSelectedTower())
        {
            return;
        }
        HighlightTile();
    }

    void OnMouseExit()
    {
        if (!TowerBuilder.Instance.HasSelectedTower())
        {
            return;
        }
        if (!tileTower)
        {
            _renderer.sharedMaterial = originalMaterial;
        }
    }

    void OnMouseDown()
    {
        if(!tileTower)
        {
            
           if (TowerBuilder.Instance.HasSelectedTower())
            {
                int cost = TowerBuilder.Instance.GetSelectedTowerCost();
                if (MoneyManager.Instance.BuyTower(cost))
                {
                    GameObject towerPrefab = TowerBuilder.Instance.GetSelectedTowerPrefab();
                    var tower = Instantiate(towerPrefab, transform.parent.position, transform.parent.rotation);
                    tileTower = tower;

                    TowerBuilder.Instance.ClearSelection();
                }
                else
                {
                    Debug.LogWarning("Selected tower cannot be afforded...");
                }
            }
        }
    }

    void HighlightTile()
    {
        if (highlightMaterial)
        {
            _renderer.sharedMaterial = highlightMaterial;
        }
    }
}
