using Laz;
using UnityEngine;

namespace Tests
{
    public class MockWorldManager : MonoBehaviour
    {
        [SerializeField] private LazCoordinatorBehaviour _coordinatorBehaviour = null;

        private void Awake()
        {
            var player = new LazPlayer();
            _coordinatorBehaviour.Initialize(player, new ILazoWrapped[] {});
        }
    }

}
