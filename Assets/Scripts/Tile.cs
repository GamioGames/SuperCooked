using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    // Serialized *****
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] private Transform itemAnchor;
    [Header("Reference")]
    [SerializeField] protected Item initialItem; 
    // Private *****
    private MeshRenderer _meshRenderer;
    protected Item _item;
    protected Action _onActionComplete;
 
    // MonoBehavior Callbacks *****
    protected virtual void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        
        if (initialItem)
        {
            GrabItem(initialItem);
        }
    }

    // Public Methods
    public virtual void TakeAction(PlayerInteraction owner, Item playerItem, Action onActionComplete)
    {
        _onActionComplete = onActionComplete;
        
        if (!_item && playerItem)
        {
            if(GrabItem(playerItem)) owner.DropItem();
        }
        else if(_item && !playerItem)
        {
           if( owner.GrabItem(_item)) DropItem();
        // For pot   
        }else if (_item && _item.TryGetComponent(out Cookware cookware) && playerItem && playerItem.TryGetComponent(out Food food))
        {
            if (cookware.AddFood(food.GetFoodData()))
            {
                owner.RemoveItem();
            }
        }
        // For Plate and food
        else if (_item && _item.TryGetComponent(out Cookware cookware2) && playerItem && playerItem.TryGetComponent(out Plate plate))
        {
            if (cookware2.TryGetDish(out FoodData foodCooked))
            {
                plate.PlateUpSoup(foodCooked.prefab);
            }
            else
            {
                Debug.Log("Food not ready");
            }
        }
        else
        {
            TakeAdvanceAction(owner);
        }
    }
    public abstract void ActionComplete();
    
    public void StopHighlight()
    {
        if (_meshRenderer)
        {
            for (int i=0; i<_meshRenderer.materials.Length; i++)
            {
                //_meshRenderer.materials[i].color = _baseColors[i];
                Material[] materials;
                (materials = _meshRenderer.materials)[i].DisableKeyword("_EMISSION");
                materials[i].globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                _meshRenderer.materials[i].SetColor("_EmissionColor", Color.black);
            }
        }else if (_skinnedMeshRenderer)
        {
            for (int i=0; i<_skinnedMeshRenderer.materials.Length; i++)
            {
                //_meshRenderer.materials[i].color = _baseColors[i];
                Material[] materials;
                (materials = _skinnedMeshRenderer.materials)[i].DisableKeyword("_EMISSION");
                materials[i].globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                _skinnedMeshRenderer.materials[i].SetColor("_EmissionColor", Color.black);
            }
        }
    }
    
    // Private Methods *****
    protected abstract void TakeAdvanceAction(PlayerInteraction owner);

    public void StartHighlight()
    {
        if (_meshRenderer)
        {
            for (int i=0; i<_meshRenderer.materials.Length; i++)
            {
                Material[] materials;
                (materials = _meshRenderer.materials)[i].EnableKeyword("_EMISSION");
                materials[i].globalIlluminationFlags = MaterialGlobalIlluminationFlags.AnyEmissive;
                _meshRenderer.materials[i].SetColor("_EmissionColor", new Color(0.2f,0.2f,0.2f));
            }
        }else if (_skinnedMeshRenderer)
        {
            for (int i=0; i<_skinnedMeshRenderer.materials.Length; i++)
            {
                Material[] materials;
                (materials = _skinnedMeshRenderer.materials)[i].EnableKeyword("_EMISSION");
                materials[i].globalIlluminationFlags = MaterialGlobalIlluminationFlags.AnyEmissive;
                _skinnedMeshRenderer.materials[i].SetColor("_EmissionColor", new Color(0.2f,0.2f,0.2f));
            }
        }
    }
    protected virtual bool GrabItem(Item item)
    {
        if (_item) return false; //This table already have item
        
        _item = item;
        _item.transform.SetParent(itemAnchor,false);
        _item.transform.localPosition = Vector3.zero;
        
        return true;
    }

    protected virtual void DropItem()
    {
        _item = null;
    }
}
