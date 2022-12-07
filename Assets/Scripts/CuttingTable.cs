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
            food.NextState();
            ActionComplete();
        }
    }

    // Public Methods *****
    public override void TakeAction(PlayerInteraction owner, Item item, Action onActionComplete)
    {
        _onActionComplete = onActionComplete;
        
        if (item & !_item)
        {
            if(GrabItem(item)) owner.DropItem();
        }
        else if(!item && _item)
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
        
        Debug.Log("Stop cutting?");
        _isCutting = false;
        _onActionComplete();
    }

    protected override void TakeAdvanceAction(PlayerInteraction owner)
    {
        Debug.Log("Start cutting");
        _timeToCut = timeToCut;
        _isCutting = true;
        owner.StartCutAnimation();
    }
}
