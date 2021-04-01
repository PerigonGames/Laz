using UnityEngine;

namespace Laz
{
    public class LazCoordinatorBehaviour : MonoBehaviour
    {
        private LazMovementBehaviour _movementBehaviour = null;
        private LazoBehaviour _lazoBehaviour = null;
        private LazModelBehavior _modelBehaviour = null;

        private void Awake()
        {
            //DEBUG - PLACEHOLDER until we get a world manager
            Initialize();
        }

        public void Initialize()
        {
            if (!TryGetComponent(out _movementBehaviour))
            {
                Debug.LogError("Laz missing LazMovementBehaviour");
            }
            
            if (!TryGetComponent(out _lazoBehaviour))
            {
                Debug.LogError("Laz missing LazoBehaviour");
            }
            
            if (!TryGetComponent(out _modelBehaviour))
            {
                Debug.LogError("Laz missing LazModelBehavior");
            }
            
            _lazoBehaviour.Initialize(_movementBehaviour);
            _modelBehaviour.Initialize(_movementBehaviour);
        }
    }
}