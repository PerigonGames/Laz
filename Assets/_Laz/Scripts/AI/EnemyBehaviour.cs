using UnityEngine;

namespace Laz
{
    public abstract class EnemyBehaviour : MonoBehaviour
    {
        [Header("Debug")] 
        [SerializeField] private bool _shouldPrintDebug = false;
        protected Vector3 _originalPosition;
        protected DebugLog debugLog = null;
        
        #region Mono

        protected virtual void Awake()
        {
            _originalPosition = transform.position;
        }

        #endregion

        protected virtual void Initialize()
        {
            debugLog = new DebugLog(gameObject.name, _shouldPrintDebug);
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

