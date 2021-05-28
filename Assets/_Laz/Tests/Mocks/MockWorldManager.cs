using Laz;
using UnityEngine;

namespace Tests
{
    public class MockWorldManager : MonoBehaviour
    {
        [SerializeField] private LazCoordinatorBehaviour _coordinatorBehaviour = null;
        [SerializeField] private PatrolBehaviour _patrolBehaviour = null;
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
            
        }
    }

}
