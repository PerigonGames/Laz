using System.Linq;

namespace Laz
{
    [System.Serializable]
    public struct LazoActivation
    {
        public string[] PuzzleTags;
        public BasePuzzleActivateBehaviour OnLazoWrappedItem;

        public void ActivateWrapItemIfNeeded(ILazoWrapped[] listOfItems)
        {
            if (ArePuzzleTagsActivated(listOfItems))
            {
                ActivatePuzzleItem();
            }
        }

        private bool ArePuzzleTagsActivated(ILazoWrapped[] listOfItems)
        {
            return PuzzleTags.All(tag =>
            {
                var items = listOfItems.Where(item => item.PuzzleTag.Equals(tag)).ToArray();
                return items.All(item => item.IsActivated);
            });
        }

        public void ActivatePuzzleItem()
        {
            OnLazoWrappedItem.OnActivated();
        }
    }
}