using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Item
{
    // Serialized *****
    [SerializeField] private FoodData foodData;
    [SerializeField] private FoodData nextFoodData;
    
    // Private *****
    private MeshFilter _mesh;

    // Public Methods *****

    public bool CanBeCut() => foodData.foodType == FoodType.Raw;
    public FoodData GetFoodData() => foodData;
    public FoodData GetNextFoodData() => nextFoodData;
}
