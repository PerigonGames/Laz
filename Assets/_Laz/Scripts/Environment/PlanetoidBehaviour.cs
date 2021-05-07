using System;
using System.Linq;
using UnityEngine;

namespace Laz
{
    public interface IPlanetoidBehaviour: ILifeCycle
    {
        public Planetoid PlanetoidModel { get; }
        public void Initialize();
        public void LazoActivated();
        public Vector3 Position { get; }
    }
    
    public class PlanetoidBehaviour : BaseWrappableBehaviour, IPlanetoidBehaviour
    {
        
        private Planetoid _planetoid;
        private PatrolBehaviour _patrolBehaviour = null;
        
        public override bool IsActivated => _planetoid.IsActivated;
        public Planetoid PlanetoidModel => _planetoid;
        public Vector3 Position => transform.position;
        
        public void Initialize()
        {
            _patrolBehaviour = GetComponent<PatrolBehaviour>();
            if (_patrolBehaviour != null)
            {
                _patrolBehaviour.Initialize();
            }
            _planetoid = new Planetoid(this);
            Reset();
        }

        public void CleanUp()
        {
            if (_patrolBehaviour != null)
            {
                _patrolBehaviour.CleanUp();
            }
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
        }

        public void Reset()
        {
            if (_patrolBehaviour != null)
            {
                _patrolBehaviour.Reset();
            }
            gameObject.SetActive(true);
        }
        
        public void LazoActivated()
        {
            PlayExplosion();
            gameObject.SetActive(false);
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