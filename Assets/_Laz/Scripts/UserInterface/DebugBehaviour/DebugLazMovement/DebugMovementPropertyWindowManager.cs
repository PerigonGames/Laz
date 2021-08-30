using UnityEngine;
using UnityEngine.InputSystem;

namespace Laz
{
    public class DebugMovementPropertyWindowManager : MonoBehaviour
    {
        private const Key DEBUG_KEY = Key.Backslash;
        
        [SerializeField] private DebugMovementParametersEditor _debugMovementParametersEditor = null;
        private bool _isEditorOpen = false;

        private void Update()
        {
            if (Keyboard.current[DEBUG_KEY].wasPressedThisFrame)
            {
                if (_isEditorOpen)
                {
                    _debugMovementParametersEditor.CloseWindow();
                }
                else
                {
                    _debugMovementParametersEditor.OpenWindow();
                }

                _isEditorOpen = !_isEditorOpen;
            }
        }
    }
}
