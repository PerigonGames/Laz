using System;
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

    public class Patrol
    {
        private readonly Queue<Vector3> _queueOfLocations = new Queue<Vector3>();
        private readonly float _speed = 0;
        private readonly Vector3[] _arrayOfLocations = new Vector3[] { };

        private Vector3? _currentDestination = null;
        
        public Vector3? CurrentDestination => _currentDestination;
        public Vector3 StartingLocation { get; } = Vector3.zero;

        public Patrol(Vector3 startingPosition, Vector3[] patrolLocations, float speed = 0f)
        {
            _speed = speed;
            _arrayOfLocations = patrolLocations;
            StartingLocation = startingPosition;
            SetupLocations();
        }
        
        public Vector3 MoveTowards(Vector3 position, float time)
        {
            var nextPosition = Vector3.MoveTowards(position, (Vector3) _currentDestination, _speed * time);
            if (position == _currentDestination)
            {
                GetNextDestination();
            }

            return nextPosition;
        }

        public void CleanUp()
        {
            _currentDestination = null;
            _queueOfLocations.Clear();
        }

        public void Reset()
        {
            SetupLocations();
        }
        
        private void SetupLocations()
        {
            _queueOfLocations.Clear();
            foreach (var location in _arrayOfLocations)
            {
                var position = new Vector3(location.x, 0, location.z);
                _queueOfLocations.Enqueue(position);
            }

            GetNextDestination();
        }
        
        private void GetNextDestination()
        {
            if (_queueOfLocations.Count > 0)
            {
                _currentDestination = _queueOfLocations.Dequeue();
                _queueOfLocations.Enqueue((Vector3) _currentDestination);
            }
        }
    }
}

