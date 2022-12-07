using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Item
{
    // Public *****
    public enum FoodType
    {
        RawFood,
        ProcessedFood
    }
    // Serialized *****
    [SerializeField] private FoodType foodType;
    [SerializeField] private Mesh nextFoodMesh;
    
    // Private *****
    private MeshFilter _mesh;
    private bool _canBeCut;
    
    // MonoBehavior Methods *****
    private void Awake()
    {
        _itemType = ItemType.Food;
        _mesh = GetComponent<MeshFilter>();
        if (foodType == FoodType.RawFood) _canBeCut = true;
    }

    // Public Methods *****
    public override void NextState()
    {
        if (foodType == FoodType.ProcessedFood) return;

        _mesh.sharedMesh = nextFoodMesh;
        foodType = FoodType.ProcessedFood;
        _canBeCut = false;
    }
    public bool CanBeCut() => _canBeCut;
}
