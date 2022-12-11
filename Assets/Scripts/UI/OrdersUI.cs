using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdersUI : MonoBehaviour
{
    // Serialized
    [SerializeField] private OrderUI orderUiPrefab;

    private List<OrderUI> _activeOrders;

    // MonoBehaviour Callbacks
    private void OnEnable()
    {
        GameManager.Instance.OnOrderCreate += GameManager_OnOrderCreate;
        GameManager.Instance.OnOrderSuccess += GameManager_OnOrderSuccess;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnOrderCreate -= GameManager_OnOrderCreate;
        GameManager.Instance.OnOrderSuccess -= GameManager_OnOrderSuccess;
    }
    
    // Private Methods
    private void GameManager_OnOrderCreate(object sender, RecipeData recipeData)
    {
        OrderUI newOrder = Instantiate(orderUiPrefab, transform);
        newOrder.SetOrder(recipeData);
        _activeOrders.Add(newOrder);
    }
    
    private void GameManager_OnOrderSuccess(object sender, EventArgs eventArgs)
    {
        if (_activeOrders.Count <= 0) return;

        _activeOrders[0].SuccessAndDestroy();

    }
}
