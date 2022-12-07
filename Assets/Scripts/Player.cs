using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    // Serialized
    [SerializeField] private float speed = 4;

    // Private
    private CharacterController _characterController;
    private Animator _anim;
    private Vector3 _currentMovement;
    private bool _isMoving;

    // MonoBehaviour Callbacks
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 move = new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical"));
        _currentMovement = move.normalized * (Time.deltaTime * speed);
        _characterController.Move(_currentMovement);
        
        // Rotation
        if (move != Vector3.zero)
        {
            _isMoving = true;
            _anim.SetBool("Running", true);
            Quaternion toRotation = Quaternion.LookRotation(_currentMovement);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 2000 * Time.deltaTime);
        }
        else
        {
            _isMoving = true;
            _anim.SetBool("Running", false);
        }
    }
    
    // Public *****
    public bool IsMoving() => _isMoving;
}
