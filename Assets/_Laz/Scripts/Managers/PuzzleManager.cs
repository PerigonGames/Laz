using UnityEngine;

namespace Laz
{
    public class PuzzleManager : MonoBehaviour
    {
        [SerializeField] 
        private LazoActivation[] _puzzleItems = null;
        
        private ILazoWrapped[] _listOfLazoWrappedItems = null;
        
        public void Initialize(ILazoWrapped[] lazoWrapped)
        {
            _listOfLazoWrappedItems = lazoWrapped;
            SetupPuzzleItems();
            SetupLazoWrappedItemsActivation();
        }

        public void Reset()
        {
            foreach (var item in _puzzleItems)
            {
                item.LazoWrappedItem.Reset();
            }
        }

        private void SetupPuzzleItems()
        {
            foreach (var item in _puzzleItems)
            {
                item.LazoWrappedItem.Initialize();
            }
        }

        private void SetupLazoWrappedItemsActivation()
        {
            foreach (var item in _listOfLazoWrappedItems)
            {
                item.OnActivated += HandleOnLazoWrappedItemActivated;
            }
        }
        
        #region Delegate

        private void HandleOnLazoWrappedItemActivated()
        {
            foreach (var puzzleItem in _puzzleItems)
            {
                puzzleItem.ActivateWrapItemIfNeeded(_listOfLazoWrappedItems);
            }
        }
        #endregion
    }

    


}