using UnityEngine;

namespace Laz
{
    public class LazCoordinatorBehaviour : MonoBehaviour
    {
        private LazMovementBehaviour _movementBehaviour = null;
        private LazoBehaviour _lazoBehaviour = null;
        private LazModelBehavior _modelBehaviour = null;

        public void Initialize(IObjectOfInterest[] objectsOfInterest)
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
            
            _lazoBehaviour.Initialize(objectsOfInterest);
            _modelBehaviour.Initialize(_movementBehaviour);
        }
    }
}