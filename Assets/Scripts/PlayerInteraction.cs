using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // Serialized *****
    [SerializeField] private Transform itemAnchor;
    // Private *****
    private Animator _anim;
    private List<Tile> _tileCloseList = new List<Tile>();
    private Tile _closestTile;
    private Item _item;

    // MonoBehaviour Callbacks *****
    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!_closestTile) return;

        if (Input.GetMouseButtonDown(0))
        {
            _closestTile.TakeAction(this, _item, StopCutAnimation);
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Only Apply for long actions
            _closestTile.ActionComplete();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Tile tile) && !_tileCloseList.Contains(tile))
        {
            _tileCloseList.Add(tile);
            
            if(_closestTile) _closestTile.StopHighlight();
            _closestTile = GetCloserTile();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Tile tile))
        {
            _tileCloseList.Remove(tile);
            
            if(_closestTile) _closestTile.StopHighlight();
            _closestTile = GetCloserTile();
        }
    }

    // Private Methods *****
    private Tile GetCloserTile()
    {
        if (_tileCloseList.Count <= 0) return null;

        Tile winnerTile = null;
        float minDistance = Mathf.Infinity;
        foreach (Tile tile in _tileCloseList)
        {
            float newDistance = Vector3.Distance(transform.position, tile.transform.position);
            if (newDistance <= minDistance)
            {
                winnerTile = tile;
                minDistance = newDistance;
            }
        }
        
        if(winnerTile) winnerTile.StartHighlight();
        return winnerTile;
    }
    
    // Public Methods *****
    public bool GrabItem(Item item)
    {
        if (_item) return false; //This player already have item

        _item = item;
        _item.transform.SetParent(itemAnchor,false);
        _item.transform.localPosition = Vector3.zero;

        return true;
    }

    public void DropItem()
    {
        _item = null;
    }

    public void RemoveItem()
    {
        Destroy(_item.gameObject);
        DropItem();
    }

    public void StartCutAnimation()
    {
        _anim.SetBool("Cutting", true);
    }

    public void StopCutAnimation()
    {
        _anim.SetBool("Cutting", false);
    }
}
