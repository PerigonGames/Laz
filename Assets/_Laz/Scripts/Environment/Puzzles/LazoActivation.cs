using System.Linq;

namespace Laz
{
    [System.Serializable]
    public struct LazoActivation
    {
        public BasePuzzleBehaviour[] PuzzleItems;
        public BaseCompletedPuzzleActivationBehaviour ItemToActivate;

        public void Initialize()
        {
            foreach (var puzzleItem in PuzzleItems)
            {
                puzzleItem.OnPuzzleCompleted += ActivateWrapItemIfNeeded;
            }
            
            ItemToActivate.Initialize();
        }

        private void ActivateWrapItemIfNeeded()
        {
            if (AreAllPuzzlesActivated())
            {
                ActivatePuzzleItem();
            }
        }

        private bool AreAllPuzzlesActivated()
        {
            return PuzzleItems.All(item => item.IsActivated);
        }

        public void ActivatePuzzleItem()
        {
            ItemToActivate.OnActivated();
        }
    }
}