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
        _timeToCut -= Time.deltaTime;

        if (_timeToCut <= 0)
        {
            Debug.Log("YOU CUUUT");
        }
    }

    // Public Methods *****
    public override void StopAction()
    {
        Debug.Log("Stop cutting?");
        if (_isCutting) _isCutting = false;
    }

    protected override void TakeAdvanceAction(PlayerInteraction owner)
    {
        Debug.Log("Start cutting");
        _timeToCut = timeToCut;
        _isCutting = true;
    }
}
