using System;
using UnityEngine;

namespace Laz
{
    public class LazMovement
    {
        private readonly ILazMovementProperty _movementProperty = null;

        private Vector2 _inputDirection;
        private bool _isMovementPressed = false;
        private Vector3 _currentDirection = Vector3.zero;
        private float _turnProgress = 0;
        private float _currentSpeed = 0;
        private float _currentMaxSpeed = 0;
        private Lazo _lazo;

        public event Action<float> OnSpeedChanges;

        public Vector3 GetCurrentDirection => _currentDirection;
        public Vector3 GetVelocity => _currentSpeed * _currentDirection;
        public float BoostTimeLimit => _movementProperty.BoostTimeLimit;

        private float CurrentSpeed
        {
            set
            {
                _currentSpeed = value;
                if (OnSpeedChanges != null)
                {
                    OnSpeedChanges(_currentSpeed);
                }
            }
        }

        public LazMovement(ILazMovementProperty movementProperty, Lazo lazo)
        {
            _movementProperty = movementProperty;
            _lazo = lazo;
            Reset();
        }

        public void CleanUp()
        {
            _lazo.OnLazoDeactivated -= SetSpeedToBaseWhenExitingLazo;
            _lazo.OnLazoLimitReached -= SetSpeedToBaseWhenExitingLazo;
            _inputDirection = Vector3.zero;
            _isMovementPressed = false;
            _currentDirection = Vector3.zero;
            _turnProgress = 0;
            _currentSpeed = 0;
            _currentMaxSpeed = 0;
        }

        public void Reset()
        {
            _lazo.OnLazoDeactivated += SetSpeedToBaseWhenExitingLazo;
            _lazo.OnLazoLimitReached += SetSpeedToBaseWhenExitingLazo;
            _currentMaxSpeed = _movementProperty.BaseMaxSpeed;
        }

        public void SetSpeedComponent()
        {
            if (_isMovementPressed)
            {
                CurrentSpeed = Math.Min(_movementProperty.Acceleration + _currentSpeed, _currentMaxSpeed);
            }
            else
            {
                CurrentSpeed = Math.Max(_currentSpeed - _movementProperty.Deceleration, 0);
            }
        }

        public void UpdateLazTurning(Vector3 normalizedVelocity)
        {
            if (_isMovementPressed)
            {
                Vector3 targetDirection = new Vector3(_inputDirection.x, 0, _inputDirection.y);

                _turnProgress += _movementProperty.CurvatureRate;
                _currentDirection = Vector3.Lerp(normalizedVelocity, targetDirection, _turnProgress);
                _turnProgress = Mathf.Clamp01(_turnProgress);
            }
        }

        public void ActivateBoost()
        {
            _currentMaxSpeed = _movementProperty.BoostSpeed;
        }

        public void DeactivateBoost()
        {
            if (_lazo.IsLazoing == true)
            {
                _currentMaxSpeed = _movementProperty.LazoMaxSpeed;
            }
            else
            {
                _currentMaxSpeed = _movementProperty.BaseMaxSpeed;
            }
        }

        private void SetSpeedToBaseWhenExitingLazo()
        {
            if (Math.Abs(_currentMaxSpeed - _movementProperty.LazoMaxSpeed) < 0.01f)
            {
                _currentMaxSpeed = _movementProperty.BaseMaxSpeed;
            }
        }

        #region Movement

        public void OnMovementPressed(Vector2 direction)
        {
            _inputDirection = direction;
            _isMovementPressed = true;
            _turnProgress = 0;
        }

        public void OnMovementCancelled()
        {
            _inputDirection = Vector2.zero;
            _isMovementPressed = false;
        }
        
        #endregion
    }
}