using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Laz
{
    public class LazMovementController : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        private Vector2 _inputDirection;

        [SerializeField]
        private float _speed = 2f;
        [SerializeField]
        private float _acceleration = 1f;
        [SerializeField]
        private float _deceleration = 1f;
        [SerializeField]
        private float _baseMaxSpeed = 5f;
        [SerializeField]
        private float _LazoMaxSpeed = 10f;
        [SerializeField]
        [Range(0, 1)]
        private float _curvatureRate = 0.1f;

        private bool _isMovementPressed = false;

        private float _currentMaxSpeed = 5f;

        private Vector3 _currentDirection = Vector3.zero;

        private float _turnProgress = 0;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _currentMaxSpeed = _baseMaxSpeed;
        }

        private void FixedUpdate()
        {
            SetSpeedComponent();
            DirectionComponent();
            UpdateRigidBody();
        }

        private void DirectionComponent()
        {
            if (_isMovementPressed == true)
            {
                Vector3 rigidbodyDirection = _rigidbody.velocity.normalized;
                Vector3 targetDirection = new Vector3(_inputDirection.x, 0, _inputDirection.y);

                _turnProgress += _curvatureRate;
                _currentDirection = Vector3.Lerp(rigidbodyDirection, targetDirection, _turnProgress);
                _turnProgress = Mathf.Clamp01(_turnProgress);
            }
        }

        private void SetSpeedComponent()
        {
            if (_isMovementPressed == true)
            {
                _speed = Math.Min(_acceleration + _speed, _currentMaxSpeed);
            }
            else
            {
                _speed = Math.Max(_speed - _deceleration, 0);
            }
        }

        private void UpdateRigidBody()
        {
            _rigidbody.velocity = _speed * _currentDirection;
        }

        /// <summary>
        /// Fired from Player input. Fired from the Inspector
        /// </summary>
        /// <param name="context"> Data with input information </param>
        public void UpdateMovementDirection(InputAction.CallbackContext context)
        {
            if (context.canceled == false)
            {
                _inputDirection = context.ReadValue<Vector2>();
                _isMovementPressed = true;
                _turnProgress = 0;
            }
            else
            {
                _inputDirection = Vector2.zero;
                _isMovementPressed = false;
            }
        }

        public void LazoBoostActivated()
        {
            _currentMaxSpeed = _LazoMaxSpeed;
        }

        public void LazoBoostDeactivated()
        {
            _currentMaxSpeed = _baseMaxSpeed;
        }
    }
}
