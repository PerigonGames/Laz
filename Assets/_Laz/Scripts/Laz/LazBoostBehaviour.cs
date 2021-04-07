using UnityEngine;
using UnityEngine.InputSystem;

namespace Laz
{
    public class LazBoostBehaviour : MonoBehaviour
    {
        [SerializeField] private LazMovementScriptableObject _lazMovementProperty = null;

        private float _elapsedBoostTime = 0;
        private bool _isBoostActivated = false;
        
        private void Awake()
        {
            ResetBoost();
            _lazMovementProperty.CurrentMaxSpeed = _lazMovementProperty.BaseMaxSpeed;
        }

        private void Update()
        {
            if (_isBoostActivated)
            {
                _elapsedBoostTime -= Time.deltaTime;
                if (_elapsedBoostTime <= 0)
                {
                    DeactivateBoost();
                    ResetBoost();
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

        private void ResetBoost()
        {
            _isBoostActivated = false;
            _elapsedBoostTime = _lazMovementProperty.BoostTimeLimit;
        }

        /// <summary>
        /// Fired from Player Input. Used in Inspecotr
        /// </summary>
        /// <param name="context">Data with input information</param>
        public void OnBoostPressed(InputAction.CallbackContext context)
        {
            if (context.performed && !_isBoostActivated)
            {
                _isBoostActivated = true;
                ActivateBoost();
            }
        }
    }
}