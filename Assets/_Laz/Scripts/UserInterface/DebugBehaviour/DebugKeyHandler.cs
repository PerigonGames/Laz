using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Laz
{
    public partial class DebugUIBehaviour
    {
        private Dictionary<Key, int> _buildKeyIndexes = new Dictionary<Key, int>
        {
            {Key.Digit1,0},
            {Key.Digit2,1},
            {Key.Digit3,2},
            {Key.Digit4,3},
            {Key.Digit5,4},
            {Key.Digit6,5},
            {Key.Digit7,6},
            {Key.Digit8,7},
            {Key.Digit9,8},
            {Key.Digit0,9}
        };
        
        
        private int GetDebugBuildIndex()
        {
            int buildIndex = -1;
            
            foreach (var buildKeyIndex in _buildKeyIndexes)
            {
                if (Keyboard.current[buildKeyIndex.Key].wasPressedThisFrame)
                {
                    buildIndex = buildKeyIndex.Value;
                }
            }

            return buildIndex;
        }
    }
}
