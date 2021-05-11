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

            if (Keyboard.current.escapeKey.isPressed)
            {
#if UNITY_EDITOR         
                UnityEditor.EditorApplication.isPlaying = false;                
#endif  
                Application.Quit();
            }
        }
    }
}

