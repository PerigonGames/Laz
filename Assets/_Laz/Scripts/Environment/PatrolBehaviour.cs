using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Laz
{
    public class PatrolBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform[] _patrolLocations = null;
        [SerializeField] private float _speed = 5;
        private Patrol _patrol;

        [SerializeField] private List<Vector3> _patrolPositions = new List<Vector3>();

        public List<Vector3> PatrolPositions
        {
            get => _patrolPositions;
            set => _patrolPositions = value;
        }

        public void Initialize()
        {
            var locations = _patrolLocations.Select(t => t.position).ToArray();
            _patrol = new Patrol(transform.position, locations, _speed);    
        }

        public void CleanUp()
        {
            transform.position = Vector3.zero;
            _patrol.CleanUp();
        }
        
        public void Reset()
        {
            transform.position = _patrol.StartingLocation;
            _patrol.Reset();
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

