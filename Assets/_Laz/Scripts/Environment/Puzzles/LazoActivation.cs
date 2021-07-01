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
                puzzleItem.Initialize();
                puzzleItem.OnPuzzleCompleted += ActivateWrapItemIfNeeded;
            }
            
            ItemToActivate.Initialize();
        }

        public void CleanUp()
        {
            foreach (var item in PuzzleItems)
            {
                item.CleanUp();
            }
            ItemToActivate.CleanUp();
        }

        public void Reset()
        {
            foreach (var item in PuzzleItems)
            {
                item.Reset();
            }
            ItemToActivate.Reset();
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