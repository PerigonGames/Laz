using UnityEngine;

namespace Laz
{
    public class LazoWallBehaviour : MonoBehaviour
    {
        private LazoPosition _lazoPosition = null;
        private Collider _collider = null;
        public LazoPosition LazoWallPosition => _lazoPosition;
        
        public void Initialize(LazoPosition lazoPosition)
        {
            _lazoPosition = lazoPosition;
            _lazoPosition.OnTimeBelowZero += HandleOnLazWallDeath;
            transform.position = lazoPosition.Position;
        }
        
        #region Mono
        private void Awake()
        {
            if (TryGetComponent(out Collider objectCollider))
            {
                _collider = objectCollider;
            }
            else
            {
                _collider = gameObject.AddComponent<Collider>();
            }
            
            SetColliderTrigger(true);
        }

        private void OnDisable()
        {
            if (_lazoPosition != null)
            {
                _lazoPosition.OnTimeBelowZero -= HandleOnLazWallDeath;
                _lazoPosition = null;
            }
        }

        #endregion
        
        
        private void SetColliderTrigger(bool on)
        {
            _collider.isTrigger = on;
        }

        private void HandleOnLazWallDeath()
        {
            gameObject.SetActive(false);
        }
    }
}