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
            foreach (var item in PuzzleItems)
            {
                item.Initialize();
                item.OnPuzzleCompleted += ActivateWrapItemIfNeeded;
            }
            
            ItemToActivate.Initialize();
        }

        public void CleanUp()
        {
            foreach (var item in PuzzleItems)
            {
                item.OnPuzzleCompleted -= ActivateWrapItemIfNeeded;
                item.CleanUp();
            }
            ItemToActivate.CleanUp();
        }

        public void Reset()
        {
            foreach (var item in PuzzleItems)
            {
                item.Reset();
                item.OnPuzzleCompleted += ActivateWrapItemIfNeeded;
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