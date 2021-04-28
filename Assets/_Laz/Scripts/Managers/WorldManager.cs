using System.Linq;
using UnityEngine;

namespace Laz
{
    public class WorldManager : MonoBehaviour
    {
        [SerializeField]
        private LazCoordinatorBehaviour _lazCoordinator = null;
        [SerializeField] 
        private ParticleEffectsObjectPooler _particleEffectsObjectPooler = null;
        [SerializeField] 
        private PuzzleManager _puzzleManager = null;
        private StateManager StateManagerInstance => StateManager.Instance;
        
        [Header("Debug")]
        [SerializeField]
        private DebugUIBehaviour debugUIBehaviour = null;

        private LazoWrappableManager _wrappableManager = null;

        public void CleanUp()
        {
            _wrappableManager.CleanUp();
            _lazCoordinator.CleanUp();
        }
        
        public void Reset()
        {
            _wrappableManager.Reset();
            _lazCoordinator.Reset();
            _puzzleManager.Reset();
        }
        
        private void Awake()
        {
            var interests = GameObject.FindGameObjectsWithTag(Tags.LazoInterest);
            var objectsOfInterest = GenerateObjectOfInterest(interests);
            var laz = new LazPlayer();
            
            StateManagerInstance.OnStateChanged += HandleOnStateChanged;
            
            _wrappableManager = new LazoWrappableManager(objectsOfInterest, StateManagerInstance);
            _lazCoordinator.Initialize(laz, _wrappableManager.WrappableObjects);
            _particleEffectsObjectPooler.Initialize(objectsOfInterest.Length);
            _puzzleManager.Initialize(_wrappableManager);
            
            // User Interface
            debugUIBehaviour.Initialize(laz.LazoTool, laz.Movement);
        }

        private void OnDestroy()
        {
            StateManagerInstance.OnStateChanged -= HandleOnStateChanged;
        }

        private IPlanetoidBehaviour[] GenerateObjectOfInterest(GameObject[] interests)
        {
            return interests.Select(x => x.GetComponent<IPlanetoidBehaviour>()).ToArray();
        }

        private void HandleOnStateChanged(State state)
        {
            if (state == State.PreGame)
            {
                CleanUp(); 
                StateManagerInstance.SetState(State.Play);
            }

            if (state == State.Play)
            {
                Reset();
            }
        }
    }
}
