using System.Linq;
using UnityEngine;

namespace Laz
{
    public class PatrolBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform[] _patrolLocations = null;
        [SerializeField] private float _speed = 5;
        private Patrol _patrol;
        
        public void Initialize()
        {
            var locations = _patrolLocations.Select(t => t.position).ToArray();
            _patrol = new Patrol(transform.position, locations, _speed);
        }

        public void CleanUp()
        {
            transform.position = Vector3.zero;
        }
        
        public void Reset()
        {
            transform.position = _patrol.StartingLocation;
        }

        private void FixedUpdate()
        {
            if (_patrol.CurrentDestination == null)
            {
                return;
            }

            transform.position = _patrol.MoveTowards(transform.position, Time.deltaTime);
        }
    }
}

