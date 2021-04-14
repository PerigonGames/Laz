using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Laz
{
    public class PlanetoidBehaviour : MonoBehaviour, IObjectOfInterest
    {
        [SerializeField] private Transform[] _patrolLocations = null;
        [SerializeField] private float _speed = 5;
        private Planetoid _planetoid;

        public event Action OnActivated;
        
        public Vector3 Position => transform.position;
        
        public void OnLazoActivated()
        {
            PlayExplosion();
            if (OnActivated != null)
            {
                OnActivated();
            }
            
            gameObject.SetActive(false);
        }

        public void Initialize()
        {
            _planetoid = new Planetoid(_speed, transform.position);
            Reset();
        }
        
        public void Reset()
        {
            _planetoid.Restart(_patrolLocations.Select(x => x.position).ToArray());
            transform.position = _planetoid.OriginalLocation;
        }
        
        public void CleanUp()
        {
            transform.position = Vector3.zero;
        }

        private void FixedUpdate()
        {
            if (_planetoid.CurrentDestination == null)
            {
                return;
            }

            transform.position = _planetoid.MoveTowards(transform.position, Time.deltaTime);
        }

        private void PlayExplosion()
        {
            var explosion = ParticleEffectsObjectPooler.Instance.PopPooledObject(ParticleSystemObjectPoolTag.PlanetoidExplosion);
            explosion.gameObject.SetActive(true);
            explosion.transform.position = transform.position;
            explosion.Play();
        }
    }

    public class Planetoid
    {
        private readonly Queue<Vector3> _queueOfLocations = new Queue<Vector3>();
        private Vector3? _currentDestination;
        private float _speed;
        private readonly Vector3 _originalLocation = Vector3.zero;

        public Vector3 OriginalLocation => _originalLocation;

        public Vector3? CurrentDestination => _currentDestination;

        public Planetoid(float speed, Vector3 spawnLocation)
        {
            _originalLocation = spawnLocation;
            _speed = speed;
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

        public void Restart(Vector3[] patrolLocations)
        {
            SetupLocations(patrolLocations);
        }
        
        private void GetNextDestination()
        {
            if (_queueOfLocations.Count > 0)
            {
                _currentDestination = _queueOfLocations.Dequeue();
                _queueOfLocations.Enqueue((Vector3) _currentDestination);
            }
        }
        private void SetupLocations(Vector3[] patrolLocations)
        {
            _queueOfLocations.Clear();
            foreach (var location in patrolLocations)
            {
                var position = new Vector3(location.x, 0, location.z);
                _queueOfLocations.Enqueue(position);
            }

            GetNextDestination();
        }
    }
    
}