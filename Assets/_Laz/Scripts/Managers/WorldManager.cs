using System.Linq;
using UnityEngine;

namespace Laz
{
    public class WorldManager : MonoBehaviour
    {
        [SerializeField]
        private LazCoordinatorBehaviour _lazCoordinator = null;
    
        
        private void Awake()
        {
            var interests = GameObject.FindGameObjectsWithTag(Tags.LazoInterest);
            var objectsOfInterest = GenerateObjectOfInterest(interests);
            var laz = new LazPlayer();
            _lazCoordinator.Initialize(laz, objectsOfInterest);
            print(laz.Movement.GetCurrentDirection);
        }

        private IObjectOfInterest[] GenerateObjectOfInterest(GameObject[] interests)
        {
            return interests.Select(x => x.GetComponent<IObjectOfInterest>()).ToArray();
        }
    }
}


