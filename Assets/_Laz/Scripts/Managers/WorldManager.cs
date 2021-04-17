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
        
        [Header("Debug")]
        [SerializeField]
        private DebugUIBehaviour debugUIBehaviour = null;

        private LazoWrappableManager _interestsManager = null;

        public void CleanUp()
        {
            _interestsManager.CleanUp();
            _lazCoordinator.CleanUp();
        }
        
        public void Reset()
        {
            _interestsManager.Reset();
            _lazCoordinator.Reset();
        }
        
        private void Awake()
        {
            var interests = GameObject.FindGameObjectsWithTag(Tags.LazoInterest);
            var objectsOfInterest = GenerateObjectOfInterest(interests);
            var laz = new LazPlayer();
            _interestsManager = new LazoWrappableManager(objectsOfInterest);
            _lazCoordinator.Initialize(laz, _interestsManager.WrappableObjects);
            debugUIBehaviour.Initialize(laz.LazoTool, laz.Movement);
            _particleEffectsObjectPooler.Initialize(objectsOfInterest.Length);
        }

        private PlanetoidBehaviour[] GenerateObjectOfInterest(GameObject[] interests)
        {
            return interests.Select(x => x.GetComponent<PlanetoidBehaviour>()).ToArray();
        }
    }
}
