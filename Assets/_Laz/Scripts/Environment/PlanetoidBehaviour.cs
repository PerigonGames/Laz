using System.Linq;
using UnityEngine;

namespace Laz
{
    public class PlanetoidBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform[] _patrolLocations = null;
        [SerializeField] private float _speed = 5;
        
        public Planetoid PlanetoidModel => _planetoid;

        private Planetoid _planetoid;
        
        public void Initialize()
        {
            var locations = _patrolLocations.Select(t => t.position).ToArray();
            _planetoid = new Planetoid(this, locations, _speed);
            Reset();
        }

        public void CleanUp()
        {
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
        }

        public void Reset()
        {
            transform.position = _planetoid.OriginalLocation;
            gameObject.SetActive(true);
        }
        
        public void OnLazoActivated()
        {
            PlayExplosion();
            gameObject.SetActive(false);
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
}