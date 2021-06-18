using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Laz
{
    public class WorldManager : MonoBehaviour
    {
        //TODO - Placeholder to get the patrolling doors
        [Title("Placeholder")]
        [SerializeField] 
        private PatrolBehaviour[] _listOfPatrollingWalls = null;
        [Title("Main Components")]
        [SerializeField]
        private LazCoordinatorBehaviour _lazCoordinator = null;
        [SerializeField] 
        private PuzzleManager _puzzleManager = null;
        [Title("Object Poolers")]
        [SerializeField] 
        private ParticleEffectsObjectPooler _particleEffectsObjectPooler = null;
        [SerializeField] 
        private LazoWallObjectPooler _lazoWallObjectPooler = null;
        private StateManager StateManagerInstance => StateManager.Instance;

        [Title("User Interface")] 
        [SerializeField]
        private LazoMeterBehaviour _lazoMeter = null;
        
        [Title("Debug")]
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

            if (_puzzleManager != null)
            {
                _puzzleManager.Initialize(_wrappableManager);
            }

            SetupObjectPoolers(objectsOfInterest.Length);
            
            //TODO - placeholder on how to handle the Patrolling objects
            foreach (var patrolItem in _listOfPatrollingWalls)
            {
                patrolItem.Initialize();
            }
            
            // User Interface
            if (_lazoMeter != null)
            {
                _lazoMeter.Initialize(laz.LazoTool);
            }

            if (debugUIBehaviour != null)
            {
                debugUIBehaviour.Initialize(laz.Movement);
            }
        }

        private void SetupObjectPoolers(int particleEffectsAmount)
        {
            if (_particleEffectsObjectPooler != null)
            {
                _particleEffectsObjectPooler.Initialize(particleEffectsAmount);
            }
            
            if (_lazoWallObjectPooler != null)
            {
                _lazoWallObjectPooler.Initialize();
            }
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
