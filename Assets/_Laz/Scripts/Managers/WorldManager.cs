using System.Linq;
using UnityEngine;

namespace Laz
{
    public class WorldManager : MonoBehaviour
    {
        //TODO - Placeholder to get the patrolling doors
        [Header("Placeholder")]
        [SerializeField] 
        private PatrolBehaviour[] _listOfPatrollingWalls = null;
        [Header("Main Components")]
        [SerializeField]
        private LazCoordinatorBehaviour _lazCoordinator = null;
        [SerializeField] 
        private ParticleEffectsObjectPooler _particleEffectsObjectPooler = null;
        [SerializeField] 
        private PuzzleManager _puzzleManager = null;
        private StateManager StateManagerInstance => StateManager.Instance;

        [Header("User Interface")] 
        [SerializeField]
        private LazoMeterBehaviour _lazoMeter = null;
        
        [Header("Debug")]
        [SerializeField]
        private DebugUIBehaviour debugUIBehaviour = null;

        private LazoWrappableManager _wrappableManager = null;

        public void CleanUp()
        {
            _wrappableManager.CleanUp();
            _lazCoordinator.CleanUp();
            //TODO - placeholder on how to handle the Patrolling objects
            foreach (var patrolItem in _listOfPatrollingWalls)
            {
                patrolItem.CleanUp();
            }
        }
        
        public void Reset()
        {
            _wrappableManager.Reset();
            _lazCoordinator.Reset();
            _puzzleManager.Reset();
            //TODO - placeholder on how to handle the Patrolling objects
            foreach (var patrolItem in _listOfPatrollingWalls)
            {
                patrolItem.Reset();
            }
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
            
            //TODO - placeholder on how to handle the Patrolling objects
            foreach (var patrolItem in _listOfPatrollingWalls)
            {
                patrolItem.Initialize();
            }
            
            // User Interface
            _lazoMeter.Initialize(laz.LazoTool);
            debugUIBehaviour.Initialize(laz.Movement);
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
