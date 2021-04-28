using System;
using System.Collections.Generic;
using UnityEngine;

namespace Laz
{
    public class Planetoid : ILazoWrapped
    {
        private readonly Queue<Vector3> _queueOfLocations = new Queue<Vector3>();
        private readonly Vector3[] _arrayOfLocations = new Vector3[] { };
        private readonly IPlanetoidBehaviour _planetoidBehaviour = null;

        private Vector3? _currentDestination = null;
        private readonly float _speed = 0;
        private bool _isActivated = false;

        public event Action OnActivated;

        public Vector3 Position => _planetoidBehaviour.Position;

        public Vector3 OriginalLocation { get; } = Vector3.zero;

        public Vector3? CurrentDestination => _currentDestination;

        public bool IsActivated => _isActivated;

        public Planetoid(IPlanetoidBehaviour planetoidBehaviour, Vector3[] patrolLocations, float speed = 0f)
        {
            _planetoidBehaviour = planetoidBehaviour;
            OriginalLocation = _planetoidBehaviour.Position;
            _arrayOfLocations = patrolLocations;
            _speed = speed;
            SetupLocations();
        }

        public void ActivateLazo()
        {
            _isActivated = true;
            _planetoidBehaviour.LazoActivated();
            if (OnActivated != null)
            {
                OnActivated();
            }
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
            _isActivated = false;
            _currentDestination = null;
            _queueOfLocations.Clear();
            _planetoidBehaviour.CleanUp();
        }

        public void Reset()
        {
            SetupLocations();
            _planetoidBehaviour.Reset();
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