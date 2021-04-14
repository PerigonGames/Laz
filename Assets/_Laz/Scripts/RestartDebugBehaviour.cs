using UnityEngine;
using UnityEngine.InputSystem;

namespace Laz
{
    public class RestartDebugBehaviour : MonoBehaviour
    {
        [SerializeField] private WorldManager _manager = null;

        private void Update()
        {
            if (Keyboard.current.rKey.isPressed)
            {
                _manager?.CleanUp();
                _manager?.Reset();
            }
        }
    }
}

