using System.Linq;

namespace Laz
{
    [System.Serializable]
    public struct LazoActivation
    {
        public BaseWrappableBehaviour[] PuzzleWrappableItems;
        public BasePuzzleActivateBehaviour OnLazoWrappedItem;

        public void ActivateWrapItemIfNeeded()
        {
            if (ArePuzzleTagsActivated())
            {
                ActivatePuzzleItem();
            }
        }

        private bool ArePuzzleTagsActivated()
        {
            return PuzzleWrappableItems.All(wrappable => wrappable.IsActivated);
        }

        public void ActivatePuzzleItem()
        {
            OnLazoWrappedItem.OnActivated();
        }
    }
}