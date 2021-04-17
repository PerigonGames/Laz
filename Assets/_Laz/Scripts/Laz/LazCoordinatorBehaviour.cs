using UnityEngine;

namespace Laz
{
    public class LazCoordinatorBehaviour : MonoBehaviour
    {
        [Header("Scriptable Objects")] 
        [SerializeField]
        private LazMovementPropertyScriptableObject _movementPropertyScriptableObject = null;
        [SerializeField] 
        private LazoPropertiesScriptableObject _lazoScriptableObject = null;

        private LazPlayer _lazPlayer = null;
        private LazMovementBehaviour _movementBehaviour = null;
        private LazoBehaviour _lazoBehaviour = null;
        private LazModelBehavior _modelBehaviour = null;
        private LazBoostBehaviour _boostBehaviour = null;

        public void Initialize(LazPlayer lazPlayer, ILazoWrapped[] objectOfInterests)
        {
            _lazPlayer = lazPlayer;
            
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

            lazPlayer.SetSpawn(transform.position);
            lazPlayer.Set(_lazoScriptableObject, objectOfInterests);
            lazPlayer.Set(_movementPropertyScriptableObject);
            
            _movementBehaviour.Initialize(lazPlayer.Movement);
            _boostBehaviour.Initialize(lazPlayer.Movement);
            _lazoBehaviour.Initialize(lazPlayer.LazoTool, _boostBehaviour);
            _modelBehaviour.Initialize(lazPlayer.Movement);
        }

        public void CleanUp()
        {
            _movementBehaviour.CleanUp();
            _lazoBehaviour.CleanUp();
            transform.position = Vector3.zero;
        }

        public void Reset()
        {
            _movementBehaviour.Reset();
            _lazoBehaviour.Reset();
            transform.position = _lazPlayer.SpawnPosition;
        }
    }
}