using UnityEngine;

namespace Laz
{
    public abstract class EnemyBehaviour : MonoBehaviour
    {
        private Vector3 _originalPosition;
        
        #region Mono

        protected virtual void Awake()
        {
            _originalPosition = transform.position;
        }

        #endregion

        protected virtual void Initialize()
        {
            
        }

        public virtual void CleanUp()
        {
            transform.position = Vector3.zero;
        }

        public virtual void Reset()
        {
            transform.position = _originalPosition;
        }
    }

   
}

