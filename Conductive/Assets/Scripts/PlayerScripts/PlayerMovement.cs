﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player behaviour variables")]
    [SerializeField] bool _characterCanMove = false;

    [Header("Movement variables")]
    [SerializeField] float _movementSpeed;
    [SerializeField] float _gravity;

    [Header("Rotation variables")]
    [SerializeField] Transform _rotationTransform;
    [SerializeField] float _rotationSpeed;
    Quaternion _targetRotation;

    CharacterController _characterController;
    PlayerManager _playerManager;


    void Awake()
    {
        _playerManager = FindObjectOfType<PlayerManager>(); 
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        ApplyGravity();

        if (_characterCanMove)
        {
            Vector3 moveDirection = GetMoveInput();
            HandleRotation(moveDirection);
            HandleMovement(moveDirection);
        }
        else
            return;
    }


    Vector3 GetMoveInput()
    {
        if (_characterController.isGrounded)
            return new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        return Vector3.zero;
    }


    void ApplyGravity()
    {
        _characterController.Move(Vector3.up * -_gravity * Time.deltaTime);
    }


    void HandleMovement(Vector3 inMoveDirection)
    {
        _characterController.Move(inMoveDirection * Time.deltaTime * _movementSpeed);
    }


    void HandleRotation(Vector3 inMoveDirection)
    {
        if (inMoveDirection == Vector3.zero)
            return;

        Quaternion currentRotation = _rotationTransform.rotation;
        _targetRotation = Quaternion.LookRotation(inMoveDirection.normalized, Vector3.up);
        _rotationTransform.rotation = Quaternion.RotateTowards(currentRotation, _targetRotation, _rotationSpeed * Time.deltaTime);
    }


    public void ManualUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
            _playerManager.CyclePlayer(true); //Cycle forward
        if (Input.GetKeyDown(KeyCode.Q))
            _playerManager.CyclePlayer(false); //Cycle backward
    }


    public void ToggleMovement(bool inShouldToggleOn)
    {
        if (inShouldToggleOn)
            _characterCanMove = true;
        else
            _characterCanMove = false;  
    }
}