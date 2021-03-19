using UnityEngine;

namespace Laz
{
    public class WorldManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerManagerBehaviour playerManagerBehaviour = null;

        private void Awake()
        {
            playerManagerBehaviour.Initialize();
        }
    }
}

/*
 * Initialize - Connect stuff
 *
 * Restart - Set values to initial values
 * CleanUp - Set Values to nil/0
 *
 * OnDestroy - Disconnect stuff
 */


