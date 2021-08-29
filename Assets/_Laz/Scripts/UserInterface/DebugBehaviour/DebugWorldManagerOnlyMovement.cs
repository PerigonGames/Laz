using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Laz
{
    public class DebugWorldManagerOnlyMovement : MonoBehaviour
    {
        [SerializeField] private LazCoordinatorBehaviour _lazCoordinator;
        
        [Title("DebugUI")]
        [SerializeField] private GameObject _debugUI = null;
        
        private StateManager StateManagerInstance => StateManager.Instance;
        private LazoWrappableManager _wrappableManager = null;

        public void CleanUp()
        {
            _lazCoordinator.CleanUp();
        }

        public void Reset()
        {
            _lazCoordinator.Reset();
        }

        private void Awake()
        {
            GameObject[] interests = GameObject.FindGameObjectsWithTag(Tags.LazoInterest);
            IPlanetoidBehaviour[] objectsOfInterest = GenerateObjectOfInterest(interests);
            
            LazPlayer laz = new LazPlayer();

            StateManagerInstance.OnStateChanged += HandleOnStateChanged;
            
            _wrappableManager = new LazoWrappableManager(objectsOfInterest, StateManager.Instance);
            _lazCoordinator.Initialize(laz, _wrappableManager.WrappableObjects);
            
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (DebugUIBehaviour.Instance != null)
            {
                DebugUIBehaviour.Instance.Initialize(laz.Movement);
            }
            else
            {
                GameObject debugUI = GameObject.Instantiate(_debugUI, Vector3.zero, Quaternion.identity);
                debugUI.GetComponent<DebugUIBehaviour>().Initialize(laz.Movement);
            }
#endif

        }
        
        private IPlanetoidBehaviour[] GenerateObjectOfInterest(GameObject[] interests)
        {
            return interests.Select(x => x.GetComponent<IPlanetoidBehaviour>()).ToArray();
        }
        
        private void HandleOnStateChanged(State state)
        {
            switch (state)
            {
                case State.PreGame:
                {
                    CleanUp();
                    StateManagerInstance.SetState(State.Play);
                    break;
                }
                case State.Play:
                {
                    Reset();
                    break;
                }
                case State.Death:
                {
                    Debug.LogWarning("Laz is dead, long live Laz!");
                    StateManagerInstance.SetState(State.PreGame);
                    break;
                }
            }
        }

        private void OnDestroy()
        {
            StateManagerInstance.OnStateChanged -= HandleOnStateChanged;
        }
    }
}
