using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodType
{
    Raw,
    Processed,
    Cooked
}
[CreateAssetMenu (menuName = "New Food")]
public class FoodData : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    public FoodType foodType;
    public GameObject prefab;
}
