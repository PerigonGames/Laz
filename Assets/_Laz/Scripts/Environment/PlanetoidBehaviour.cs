using UnityEngine;

namespace Laz
{
    public interface IPlanetoidBehaviour
    {
        Planetoid PlanetoidModel { get; }
        void Initialize();
        void LazoActivated();
        Vector3 Position { get; }
        void Reset();
        void CleanUp();
    }
    
    public class PlanetoidBehaviour : BasePuzzleBehaviour, IPlanetoidBehaviour
    {
        private Planetoid _planetoid;
        private PatrolBehaviour _patrolBehaviour = null;
        public Planetoid PlanetoidModel => _planetoid;
        public override bool IsActivated => _planetoid.IsActivated;
        public Vector3 Position => transform.position;
        
        public override void Initialize()
        {
            base.Initialize();
            _patrolBehaviour = GetComponent<PatrolBehaviour>();
            if (_patrolBehaviour != null)
            {
                _patrolBehaviour.Initialize();
            }
            _planetoid = new Planetoid(this);
            Reset();
        }

        public override void CleanUp()
        {
            base.CleanUp();
            if (_patrolBehaviour != null)
            {
                _patrolBehaviour.CleanUp();
            }
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
        }

        public override void Reset()
        {
            base.Reset();
            if (_patrolBehaviour != null)
            {
                _patrolBehaviour.Reset();
            }
            gameObject.SetActive(true);
        }
        
        public void LazoActivated()
        {
            Activate();
            PlayExplosion();
            gameObject.SetActive(false);
        }

        private void PlayExplosion()
        {
            if (ParticleEffectsObjectPooler.Instance != null)
            {
                var explosion = ParticleEffectsObjectPooler.Instance.PopPooledObject(ParticleSystemObjectPoolTag.PlanetoidExplosion);
                explosion.gameObject.SetActive(true);
                explosion.transform.position = transform.position;
                explosion.Play();
            }
            
        }

    }
}