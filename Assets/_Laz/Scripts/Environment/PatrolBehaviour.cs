using System.Collections.Generic;
using UnityEngine;

namespace Laz
{
    public class PatrolBehaviour : MonoBehaviour
    {
        [SerializeField] private List<Vector3> _patrolPositions = new List<Vector3>();
        [SerializeField] private float _speed = 5;
        private Patrol _patrol;
        
        public List<Vector3> PatrolPositions
        {
            get => _patrolPositions;
            set => _patrolPositions = value;
        }

        public void Initialize()
        {
            _patrol = new Patrol(transform.position, PatrolPositions.ToArray(), _speed);    
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

