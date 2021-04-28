using UnityEngine;

namespace Laz
{
    public class PuzzleManager : MonoBehaviour
    {
        [SerializeField] 
        private LazoActivation[] _puzzleItems = null;

        private LazoWrappableManager _lazoWrappableManager = null;
        
        public void Initialize(LazoWrappableManager wrappableManager)
        {
            _lazoWrappableManager = wrappableManager;
            _lazoWrappableManager.OnWrappableActivated += HandleOnLazoWrappedItemActivated;
            SetupPuzzleItems();
        }

        public void Reset()
        {
            foreach (var item in _puzzleItems)
            {
                item.OnLazoWrappedItem.Reset();
            }
        }

        private void OnDestroy()
        {
            _lazoWrappableManager.OnWrappableActivated -= HandleOnLazoWrappedItemActivated;
        }

        private void SetupPuzzleItems()
        {
            foreach (var item in _puzzleItems)
            {
                item.OnLazoWrappedItem.Initialize();
            }
        }

        #region Delegate

        private void HandleOnLazoWrappedItemActivated()
        {
            foreach (var puzzleItem in _puzzleItems)
            {
                puzzleItem.ActivateWrapItemIfNeeded();
            }
        }
        #endregion
    }

    


}