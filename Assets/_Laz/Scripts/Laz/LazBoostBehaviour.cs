using UnityEngine;
using UnityEngine.InputSystem;

namespace Laz
{
    public class LazBoostBehaviour : MonoBehaviour
    {
        [SerializeField] private LazMovementScriptableObject _lazMovementProperty = null;

        private float _elapsedBoostTime = 0;
        private bool _isBoostActivated = false;

        public bool IsBoostActivated
        {
            get => _isBoostActivated;
            set
            {
                _isBoostActivated = value;
                if (_isBoostActivated)
                {
                    ActivateBoost();
                }
                else
                {
                    DeactivateBoost();
                    ResetBoostTime();
                }
            }
        }

        private void Awake()
        {
            ResetBoostTime();
            _lazMovementProperty.CurrentMaxSpeed = _lazMovementProperty.BaseMaxSpeed;
        }

        private void Update()
        {
            if (IsBoostActivated)
            {
                _elapsedBoostTime -= Time.deltaTime;
                if (_elapsedBoostTime <= 0)
                {
                    IsBoostActivated = false;
                }
            }
        }

        private void ActivateBoost()
        {
            _lazMovementProperty.CurrentMaxSpeed = _lazMovementProperty.BoostSpeed;
        }

        private void DeactivateBoost()
        {
            _lazMovementProperty.CurrentMaxSpeed = _lazMovementProperty.BaseMaxSpeed;
        }

        private void ResetBoostTime()
        {
            _elapsedBoostTime = _lazMovementProperty.BoostTimeLimit;
        }

        /// <summary>
        /// Fired from Player Input. Used in Inspecotr
        /// </summary>
        /// <param name="context">Data with input information</param>
        public void OnBoostPressed(InputAction.CallbackContext context)
        {
            if (context.performed && !IsBoostActivated)
            {
                IsBoostActivated = true;
            }
        }
    }
}