
using UnityEngine;

namespace Laz
{
    public class ObjectOfInterestsManager
    {
        private IObjectOfInterest[] _interests = null;
        private int numberOfObjectsToActivate = 0;

        public ObjectOfInterestsManager(IObjectOfInterest[] objectOfInterests)
        {
            numberOfObjectsToActivate = objectOfInterests.Length;
            _interests = objectOfInterests;
            foreach (var interest in _interests)
            {
                interest.OnActivated += HandleOnInterestActivated;
            }
        }

        private void HandleOnInterestActivated()
        {
            numberOfObjectsToActivate--;
            if (numberOfObjectsToActivate <= 0)
            {
                Debug.Log("You Have Won");
            }
        }
    }
}