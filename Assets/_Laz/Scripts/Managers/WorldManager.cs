using UnityEngine;

namespace Laz
{
    public class WorldManager : MonoBehaviour
    {
        [SerializeField]
        private LazCoordinatorBehaviour _lazCoordinator = null;

        private void Awake()
        {
            _lazCoordinator.Initialize();
        }
    }
}


