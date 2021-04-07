using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Laz
{
    public class LazMovementBehaviour : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private ILazMovement _lazMovementProperty = null;
        
        private Vector2 _inputDirection;
        private bool _isMovementPressed = false;
        private Vector3 _currentDirection = Vector3.zero;
        private float _turnProgress = 0;
        private float _currentSpeed = 0;

        public Vector3 GetCurrentDirection => _currentDirection;

        public void Initialize(ILazMovement movement)
        {
            _lazMovementProperty = movement;
        }
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            SetSpeedComponent();
            DirectionComponent();
            UpdateRigidBody();
        }

        private void DirectionComponent()
        {
            if (_isMovementPressed)
            {
                Vector3 rigidbodyDirection = _rigidbody.velocity.normalized;
                Vector3 targetDirection = new Vector3(_inputDirection.x, 0, _inputDirection.y);

                _turnProgress += _lazMovementProperty.CurvatureRate;
                _currentDirection = Vector3.Lerp(rigidbodyDirection, targetDirection, _turnProgress);
                _turnProgress = Mathf.Clamp01(_turnProgress);
            }
        }

        private void SetSpeedComponent()
        {
            if (_isMovementPressed)
            {
                _currentSpeed = Math.Min(_lazMovementProperty.Acceleration + _currentSpeed, _lazMovementProperty.CurrentMaxSpeed);
            }
            else
            {
                _currentSpeed = Math.Max(_currentSpeed - _lazMovementProperty.Deceleration, 0);
            }
        }

        private void UpdateRigidBody()
        {
            _rigidbody.velocity = _currentSpeed * _currentDirection;
        }

        /// <summary>
        /// Fired from Player input. Fired from the Inspector
        /// </summary>
        /// <param name="context"> Data with input information </param>
        public void UpdateMovementDirection(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _inputDirection = context.ReadValue<Vector2>();
                _isMovementPressed = true;
                _turnProgress = 0;
            }
            
            if (context.canceled)
            {
                _inputDirection = Vector2.zero;
                _isMovementPressed = false;
            }
        }
    }
}
