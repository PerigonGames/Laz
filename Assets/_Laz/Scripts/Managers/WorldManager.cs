using System.Linq;
using UnityEngine;

namespace Laz
{
    public class WorldManager : MonoBehaviour
    {
        [SerializeField]
        private LazCoordinatorBehaviour _lazCoordinator = null;
        [SerializeField] 
        private ParticleEffectsObjectPooler _particleEffectsObjectPooler = null;
        
        [SerializeField]
        private DebugUIBehaviour debugUIBehaviour = null;

        private ObjectOfInterestsManager _interestsManager = null;

        private void Awake()
        {
            var interests = GameObject.FindGameObjectsWithTag(Tags.LazoInterest);
            var objectsOfInterest = GenerateObjectOfInterest(interests);
            var laz = new LazPlayer();
            _interestsManager = new ObjectOfInterestsManager(objectsOfInterest);
            _lazCoordinator.Initialize(laz, objectsOfInterest);
            debugUIBehaviour.Initialize(laz.LazoTool, laz.Movement);
            _particleEffectsObjectPooler.Initialize(objectsOfInterest.Length);
        }

        private IObjectOfInterest[] GenerateObjectOfInterest(GameObject[] interests)
        {
            return interests.Select(x => x.GetComponent<IObjectOfInterest>()).ToArray();
        }
    }
}
