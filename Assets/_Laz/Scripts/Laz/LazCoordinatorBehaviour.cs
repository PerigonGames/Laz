using System;
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

        private ILazMovementProperty _movementProperties = null;
        private ILazoProperties _lazoProperties = null;
        
        private LazPlayer _lazPlayer = null;
        private LazMovementBehaviour _movementBehaviour = null;
        private LazoBehaviour _lazoBehaviour = null;
        private LazModelBehavior _modelBehaviour = null;
        private LazBoostBehaviour _boostBehaviour = null;

        public void Initialize(LazPlayer lazPlayer, 
            ILazoWrapped[] objectOfInterests = null, 
            ILazMovementProperty movementProperty = null,
            ILazoProperties lazoProperties = null)
        {
            _lazPlayer = lazPlayer;
            if (objectOfInterests == null)
            {
                objectOfInterests = new ILazoWrapped[] { };
            }
            
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

            _lazPlayer.SetSpawn(transform.position);

            _lazoProperties = lazoProperties ?? _lazoScriptableObject;
            _lazPlayer.SetLazo(_lazoProperties, objectOfInterests, _boostBehaviour);

            _movementProperties = movementProperty ?? _movementPropertyScriptableObject;
            _lazPlayer.SetMovement(_movementProperties, lazPlayer.LazoTool);
            
            _movementBehaviour.Initialize(_lazPlayer.Movement);
            _boostBehaviour.Initialize(_lazPlayer.Movement);
            _lazoBehaviour.Initialize(_lazPlayer.LazoTool);
            _modelBehaviour.Initialize(_lazPlayer.Movement);
        }

        public void CleanUp()
        {
            _movementBehaviour.CleanUp();
            _lazoBehaviour.CleanUp();
            _boostBehaviour.CleanUp();
            transform.position = Vector3.zero;
        }

        public void Reset()
        {
            _movementBehaviour.Reset();
            _lazoBehaviour.Reset();
            _boostBehaviour.Reset();
            transform.position = _lazPlayer.SpawnPosition;
        }

        public void KillLaz()
        {
            _lazPlayer.KillLaz();
        }
    }
}