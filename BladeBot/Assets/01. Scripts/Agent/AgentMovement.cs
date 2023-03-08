using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AgentMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 8f, _gravity = -9.8f;
    private CharacterController _characterController;

    private Vector3 _movementVelocity;

    public Vector3 MovementVelocity => _movementVelocity; //평면 속도
    private float _verticalVelocity; //중력속도

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); // 0, 1만
        float vertical = Input.GetAxisRaw("Vertical");

        _movementVelocity = new Vector3(horizontal, 0, vertical);
    }

    // 플레이어 이속계산
    private void CalculatePlayerMovement()
    {
        _movementVelocity.Normalize();
        // == _movementVelocity = _movementVelocity.normalized;
        _movementVelocity *= _moveSpeed * Time.deltaTime;
        _movementVelocity = Quaternion.Euler(0, -45f, 0) * _movementVelocity;

        if (_movementVelocity.sqrMagnitude > 0)
        {
            // 가야할 방향 보게 하기 
            transform.rotation = Quaternion.LookRotation(_movementVelocity);
        }
    }

    private void StopImmediately()
    {
        
    }

    private void FixedUpdate()
    {
        CalculatePlayerMovement();

        if (_characterController.isGrounded == false)
        {
            _verticalVelocity = _gravity * Time.fixedDeltaTime;
        }
        else
        {
            // 0.3은 하드코딩
            _verticalVelocity = _gravity * 0.3f * Time.fixedDeltaTime;
        }

        Vector3 move = _movementVelocity + _verticalVelocity * Vector3.up;
        _characterController.Move(move);
    }
}
