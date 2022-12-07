using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    // Serialized *****
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] private Transform itemAnchor;
    // Private *****
    private MeshRenderer _meshRenderer;
    private GameObject _item;

    // MonoBehavior Callbacks
    protected virtual void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    // Public Methods
    public void TakeAction(PlayerInteraction owner, GameObject item)
    {
        if (item & !_item)
        {
            if(GrabItem(item)) owner.DropItem();
        }
        else if(!item && _item)
        {
           if( owner.GrabItem(_item)) DropItem();
        }
        else
        {
            TakeAdvanceAction(owner);
        }
    }
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
    
    public abstract void StopAction();
    
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
    private bool GrabItem(GameObject item)
    {
        if (_item) return false; //This table already have item
        
        _item = item;
        _item.transform.SetParent(itemAnchor,false);

        return true;
    }

    private void DropItem()
    {
        _item = null;
    }
}
