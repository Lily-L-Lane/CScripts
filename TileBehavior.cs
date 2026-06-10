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
        if (highlightMaterial)
        {
            _renderer.sharedMaterial = highlightMaterial;
        }
    }

    void OnMouseExit()
    {
        if (!tileTower)
        {
            _renderer.sharedMaterial = originalMaterial;
        }
    }

    void OnMouseDown()
    {
        if(!tileTower)
        {
           if (towerPrefab)
            {
                HighlightTile();
                var tower = Instantiate(towerPrefab, transform.parent.position, transform.parent.rotation);
                tileTower = tower;
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
