using UnityEngine;

namespace Laz
{
    public interface IBoost
    {
        bool IsBoostActivated { get; set; }
    }
    
    public class LazBoostBehaviour : MonoBehaviour, IBoost
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
        
        
        /// <summary>
        /// Resets the amount of boost time
        /// </summary>
        public void ResetBoostTime()
        {
            _elapsedBoostTime = _lazMovementProperty.BoostTimeLimit;
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
    }
}