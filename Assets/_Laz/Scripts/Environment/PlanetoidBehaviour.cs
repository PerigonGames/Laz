using System;
using System.Collections.Generic;
using UnityEngine;

namespace Laz
{
    public class PlanetoidBehaviour : MonoBehaviour, IObjectOfInterest
    {
        [SerializeField] private Transform[] _patrolLocations = null;

        [SerializeField] private float _speed = 5;
        
        public event Action OnActivated;
        public Vector3 Position => transform.position;
        
        private readonly Queue<Vector3> _queueOfLocations = new Queue<Vector3>();
        private Vector3? _currentDestination;
        
        public void OnLazoActivated()
        {
            PlayExplosion();
            if (OnActivated != null)
            {
                OnActivated();
            }
            
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            SetupLocations();
        }

        private void FixedUpdate()
        {
            if (_currentDestination == null)
            {
                return;
            }
            
            transform.position = Vector3.MoveTowards(transform.position, (Vector3) _currentDestination, _speed * Time.deltaTime);
            if (transform.position == _currentDestination)
            {
                GetNextDestination();
            }
        }

        private void PlayExplosion()
        {
            var explosion = ParticleEffectsObjectPooler.Instance.PopPooledObject(ParticleSystemObjectPoolTag.PlanetoidExplosion);
            explosion.gameObject.SetActive(true);
            explosion.transform.position = transform.position;
            explosion.Play();
        }

        private void SetupLocations()
        {
            foreach (var location in _patrolLocations)
            {
                var patrolPosition = location.position;
                var position = new Vector3(patrolPosition.x, 0, patrolPosition.z);
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