using Laz;
using UnityEngine;

namespace Tests
{
    public class MockWorldManager : MonoBehaviour
    {
        
        [SerializeField] private LazCoordinatorBehaviour _coordinatorBehaviour = null;
        [SerializeField] private PatrolBehaviour _patrolBehaviour = null;
        [SerializeField] private PlanetoidBehaviour _planetoidBehaviour = null;
        [SerializeField] private LazoWallObjectPooler _wallObjectPool = null;
        [SerializeField] private PuzzleManager _puzzleManager = null;
        [SerializeField] private CheckPointManager _checkPointManager = null;
        
        public ILazoWrapped Wrappable => _planetoidBehaviour.PlanetoidModel;
        
        private void Awake()
        {
            var player = new LazPlayer();
            if (_coordinatorBehaviour != null)
            {
                _coordinatorBehaviour.Initialize(player, new ILazoWrapped[] { });
            }

            if (_patrolBehaviour != null)
            {
                _patrolBehaviour.Initialize();
            }

            if (_planetoidBehaviour != null)
            {
                _planetoidBehaviour.Initialize();
            }

            if (_wallObjectPool != null)
            {
                _wallObjectPool.Initialize();
            }
            else
            {
                Debug.LogWarning("Must Initialize LazoWallObjectPooler if using Lazo Tool");
            }

            if (_puzzleManager != null)
            {
                _puzzleManager.Initialize();
            }

            if(_checkPointManager != null)
            {
                _checkPointManager.Initialize(player);
            }
        }
    }

}
