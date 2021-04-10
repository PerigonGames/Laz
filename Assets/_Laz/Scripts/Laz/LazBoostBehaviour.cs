using UnityEngine;

namespace Laz
{
    public interface IBoost
    {
        bool IsBoostActivated { get; set; }
    }
    
    public class LazBoostBehaviour : MonoBehaviour, IBoost
    {
        private LazMovement _lazMovement = null;
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
                    _lazMovement.ActivateBoost();
                }
                else
                {
                    _lazMovement.DeactivateBoost();
                    ResetBoostTime();
                }
            }
        }

        public void Initialize(LazMovement movementProperty)
        {
            _lazMovement = movementProperty;
            ResetBoostTime();
        }
        
        /// <summary>
        /// Resets the amount of boost time
        /// </summary>
        public void ResetBoostTime()
        {
            _elapsedBoostTime = _lazMovement.BoostTimeLimit;
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
    }
}