using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingTable : Tile
{
    // Serialized Field
    [SerializeField] private float timeToCut = 2;
    // Private *****
    private float _timeToCut;
    private bool _isCutting;
    
    // MonoBehavior Methods
    private void Start()
    {
        _timeToCut = timeToCut;
    }

    private void Update()
    {
        if (!_isCutting) return;
        
        _timeToCut -= Time.deltaTime;

        if (_timeToCut <= 0 && _item.TryGetComponent(out Food food) && food.CanBeCut())
        {
            if (CraftFood(food.GetNextFoodData()))
            {
                ActionComplete();
                _timeToCut = timeToCut;
            }
        }
    }

    // Public Methods *****
    public override void TakeAction(PlayerInteraction owner, Item playerItem, Action onActionComplete)
    {
        _onActionComplete = onActionComplete;
        
        if (playerItem & !_item)
        {
            if(GrabItem(playerItem)) owner.DropItem();
        }
        else if(!playerItem && _item)
        {
            if (_item.TryGetComponent(out Food food) && food.CanBeCut())
            {
                TakeAdvanceAction(owner);
            }else
            {
                if( owner.GrabItem(_item)) DropItem();
            }
        }
    }

    public override void ActionComplete()
    {
        if (!_item && !_isCutting) return;
        
        _isCutting = false;
        _onActionComplete();
    }

    // Private Methods *****
    protected override void TakeAdvanceAction(PlayerInteraction owner)
    {
        _timeToCut = timeToCut;
        _isCutting = true;
        owner.StartCutAnimation();
    }
    
    /// <summary>
    /// Replace current item for new one
    /// </summary>
    private bool CraftFood(FoodData newFoodData)
    {
        if (!_item) return false;

        GameObject newFoodClone = Instantiate(newFoodData.prefab, _item.transform.parent,false);
        Destroy(_item.gameObject);
        _item = newFoodClone.GetComponent<Item>();
        return true;
    }
}
