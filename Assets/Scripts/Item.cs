using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    // Public *****
    public enum ItemType
    {
        Item,
        Food,
        Dish,
    }
    
    // Private *****
    protected ItemType _itemType;
    
    // Public Methods
    public abstract void NextState();
    public ItemType GetItemType() => _itemType;
}
