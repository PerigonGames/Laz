using System.Linq;
using UnityEngine;

namespace Laz
{
    public class PuzzleManager : MonoBehaviour
    {
        [SerializeField] 
        private LazoActivation[] _puzzleItems = null;
        
        public void Initialize()
        {
            SetupPuzzleItems();
        }

        public void CleanUp()
        {
            foreach (var item in _puzzleItems)
            {
                item.CleanUp();
            }
        }

        public void Reset()
        {
            foreach (var item in _puzzleItems)
            {
                item.Reset();
            }
        }

        private void SetupPuzzleItems()
        {
            foreach (var item in _puzzleItems)
            {
                if (item.PuzzleItems.Contains(null))
                {
                    Debug.LogWarning("Empty Puzzle Wrappable items Within the Puzzle Manager");
                }

                item.Initialize();
            }
        }
    }
}