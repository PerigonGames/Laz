using System.Linq;
using UnityEngine;

namespace Laz
{
    public class WorldManager : MonoBehaviour
    {
        [SerializeField]
        private LazCoordinatorBehaviour _lazCoordinator = null;

        [SerializeField] 
        private LazoUI _lazoUI = null;
        
        
        private void Awake()
        {
            var interests = GameObject.FindGameObjectsWithTag(Tags.LazoInterest);
            var objectsOfInterest = GenerateObjectOfInterest(interests);
            var laz = new LazPlayer();
            _lazCoordinator.Initialize(laz, objectsOfInterest);
            _lazoUI.Initialize(laz.LazoTool);
        }

        private IObjectOfInterest[] GenerateObjectOfInterest(GameObject[] interests)
        {
            return interests.Select(x => x.GetComponent<IObjectOfInterest>()).ToArray();
        }
    }
}


