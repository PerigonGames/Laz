using UnityEngine;

namespace Laz
{
    public interface IBoost
    {
        bool IsBoostActivated { get; }
        void SetBoostActive(bool activate);
    }
    
    public class LazBoostBehaviour : MonoBehaviour, IBoost
    {
        private LazMovement _lazMovement = null;
        private float _elapsedBoostTime = 0;
        private bool _isBoostActivated = false;

        public bool IsBoostActivated => _isBoostActivated;

        public void Initialize(LazMovement movementProperty)
        {
            _lazMovement = movementProperty;
        }

        public void CleanUp()
        {
            _elapsedBoostTime = 0;
            _isBoostActivated = false;
        }

        public void Reset()
        {
            ResetBoostTime();
        }

        public void SetBoostActive(bool activate)
        {
            _isBoostActivated = activate;
            if (_isBoostActivated)
            {
                ResetBoostTime();
                _lazMovement.ActivateBoost();
            }
            else
            {
                _lazMovement.DeactivateBoost();
            }
        }
        
        private void ResetBoostTime()
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
                    SetBoostActive(false);
                }
            }
        }
    }
}