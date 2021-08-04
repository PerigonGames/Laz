using UnityEngine.InputSystem;

namespace Laz
{
    public partial class DebugUIBehaviour
    {
        private int GetDebugBuildIndex()
        {
            int buildIndex = -1;
            
            if (Keyboard.current[Key.Digit1].wasPressedThisFrame)
            {
                buildIndex = 0;
            }
            else if (Keyboard.current[Key.Digit2].wasPressedThisFrame)
            {
                buildIndex = 1;
            }
            else if (Keyboard.current[Key.Digit3].wasPressedThisFrame)
            {
                buildIndex = 2;
            }
            else if (Keyboard.current[Key.Digit4].wasPressedThisFrame)
            {
                buildIndex = 3;
            }
            else if (Keyboard.current[Key.Digit5].wasPressedThisFrame)
            {
                buildIndex = 4;
            }
            else if (Keyboard.current[Key.Digit6].wasPressedThisFrame)
            {
                buildIndex = 5;
            }
            else if (Keyboard.current[Key.Digit7].wasPressedThisFrame)
            {
                buildIndex = 6;
            }
            else if (Keyboard.current[Key.Digit8].wasPressedThisFrame)
            {
                buildIndex = 7;
            }
            else if (Keyboard.current[Key.Digit9].wasPressedThisFrame)
            {
                buildIndex = 8;
            }
            else if (Keyboard.current[Key.Digit0].wasPressedThisFrame)
            {
                buildIndex = 9;
            }

            return buildIndex;
        }
    }
}
