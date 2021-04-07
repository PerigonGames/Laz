using UnityEngine;

namespace Laz
{
    public class LazCoordinatorBehaviour : MonoBehaviour
    {
        [Header("Scriptable Objects")] 
        [SerializeField]
        private LazMovementScriptableObject _movementScriptableObject = null;
        [SerializeField] 
        private LazoPropertiesScriptableObject _lazoScriptableObject = null;
        
        private LazMovementBehaviour _movementBehaviour = null;
        private LazoBehaviour _lazoBehaviour = null;
        private LazModelBehavior _modelBehaviour = null;
        private LazBoostBehaviour _boostBehaviour = null;

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
            
            if (!TryGetComponent(out _boostBehaviour))
            {
                Debug.LogError("Laz missing LazBoostBehaviour");
            }
            
            _movementBehaviour.Initialize(_movementScriptableObject);
            _boostBehaviour.Initialize(_movementScriptableObject);
            _lazoBehaviour.Initialize(_lazoScriptableObject, objectsOfInterest, _boostBehaviour);
            _modelBehaviour.Initialize(_movementBehaviour);
        }
    }
}