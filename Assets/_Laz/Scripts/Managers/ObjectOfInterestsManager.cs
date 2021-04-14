using UnityEngine;

namespace Laz
{
    public class ObjectOfInterestsManager: ILifeCycle
    {
        private IObjectOfInterest[] _interests = null;
        private int numberOfObjectsToActivate = 0;

        public ObjectOfInterestsManager(IObjectOfInterest[] objectOfInterests)
        {
            numberOfObjectsToActivate = objectOfInterests.Length;
            _interests = objectOfInterests;
            foreach (var interest in _interests)
            {
                if (interest is PlanetoidBehaviour planetoid)
                {
                    planetoid.Initialize();
                }
                
                interest.OnActivated += HandleOnInterestActivated;
            }
        }

        public void CleanUp()
        {
            foreach (var interest in _interests)
            {
                interest.CleanUp();
            }
        }

        public void Reset()
        {
            foreach (var interest in _interests)
            {
                interest.Reset();
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