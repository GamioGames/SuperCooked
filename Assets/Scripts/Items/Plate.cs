using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : Item
{
    // Serialized
    [SerializeField] private Transform itemAnchor;
    // Private *****
    private GameObject _food;
    private FoodData _foodData;

    // Public Methods
    public void PlateUpSoup(FoodData foodData)
    {
        if (_food) return;

        _foodData = foodData;
        _food = Instantiate(foodData.prefab, itemAnchor, false);
        _food.transform.localPosition = Vector3.zero;
    }

    public void Unplate()
    {
        if (!_food) return;

        Destroy(_food);
        _foodData = null;
    }

    public bool IsCookedFood() => _foodData != null;
}
