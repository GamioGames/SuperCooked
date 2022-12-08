using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : Item
{
    // Serialized
    [SerializeField] private Transform itemAnchor;
    // Private *****
    private GameObject _food;

    // Public Methods
    public void PlateUpSoup(GameObject soup)
    {
        if (_food) return;

        _food = Instantiate(soup, itemAnchor, false);
        _food.transform.localPosition = Vector3.zero;
    }

    public void Unplate()
    {
        if (!_food) return;
        
    }

}
