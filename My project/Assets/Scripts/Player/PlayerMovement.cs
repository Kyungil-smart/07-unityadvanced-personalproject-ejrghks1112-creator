using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerActionInput _input;
    private Rigidbody2D _rb;
    Vector2 _moveInput;
    
    [SerializeField] float _moveSpeed;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        _input.PlayerAction.Enable();
    }

    private void Update()
    {
        _moveInput = _input.PlayerAction.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _moveInput * _moveSpeed;
    }

    private void OnDisable()
    {
        _input.PlayerAction.Disable();
    }

    void Init()
    {
        _input = new PlayerActionInput();
        _rb = GetComponent<Rigidbody2D>();
    }
}
