using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdersUI : MonoBehaviour
{
    // Serialized
    [SerializeField] private OrderUI orderUiPrefab;
    
    // MonoBehaviour Callbacks
    private void OnEnable()
    {
        GameManager.Instance.OnOrderCreate += GameManager_OnOrderCreate;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnOrderCreate -= GameManager_OnOrderCreate;
    }
    
    // Private Methods
    private void GameManager_OnOrderCreate(object sender, RecipeData recipeData)
    {
        OrderUI newOrder = Instantiate(orderUiPrefab, transform);
        newOrder.SetOrder(recipeData);
    }
}
