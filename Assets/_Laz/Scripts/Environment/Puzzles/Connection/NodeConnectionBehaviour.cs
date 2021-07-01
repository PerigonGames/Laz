using UnityEngine;

namespace Laz
{
    [RequireComponent(typeof(Collider))]
    public class NodeConnectionBehaviour : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem = null;
        [SerializeField] private Transform _meshTransform = null;
        
        private Node _node = null;
        private LazoBehaviour _lazo = null;
        
        public void Initialize(Node node)
        {
            ClearParticleSystem();
            _node = node;
            _node.OnCanActivateChanged += HandleOnCanActivateChanged;
            _node.OnNodeCompleted += HandleOnNodeCompleted;
        }

        private void CleanUp()
        {
            ClearParticleSystem();
            HandleOnNodeCompleted();
            _node.OnCanActivateChanged -= HandleOnCanActivateChanged;
            _node = null;
        }

        private void Reset()
        {
            
        }

        private void ClearParticleSystem()
        {
            _particleSystem.Stop();
            _particleSystem.Clear();
        }

        private void HandleOnCanActivateChanged(bool canActivate)
        {
            if (canActivate)
            {
                _meshTransform.localScale = Vector3.one * 0.75f;
            }
            else
            {
                _meshTransform.localScale = Vector3.one * 0.3f;
            }
        }

        private void HandleOnNodeCompleted()
        {
            _meshTransform.localScale = Vector3.zero;
            ClearParticleSystem();
            _lazo = null;
        }

        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            var lazo = other.GetComponent<LazoBehaviour>();
            if (lazo != null && lazo.IsLazoing && _node.CanActivate)
            {
                _lazo = lazo;
                _particleSystem.Play();
                _node.IsActive = true;
            }
        }

        private void Update()
        {
            if (_lazo != null && !_lazo.IsLazoing)
            {
                ClearParticleSystem();
                _node.IsActive = false;
                _lazo = null;
            }
        }
    }
}