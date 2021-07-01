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

        public void CleanUp()
        {
            ClearParticleSystem();
            HandleOnNodeCompleted();
            _node.OnCanActivateChanged -= HandleOnCanActivateChanged;
            _node.OnNodeCompleted += HandleOnNodeCompleted;
            _node = null;
        }

        public void Reset()
        {
            HandleOnCanActivateChanged(false);
        }

        private void ClearParticleSystem()
        {
            _particleSystem.Stop();
            _particleSystem.Clear();
        }
        
        private void ActivateNodeIfNeeded(LazoBehaviour lazo)
        {
            if (lazo is {IsLazoing: true} && _node.CanActivate)
            {
                _lazo = lazo;
                _particleSystem.Play();
                _node.IsActive = true;
            }
        }
        
        #region Delegate
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
        #endregion
        
        #region Mono
        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            var lazo = other.GetComponent<LazoBehaviour>();
            ActivateNodeIfNeeded(lazo);
        }
        
        private void OnTriggerStay(Collider other)
        {
            var lazo = other.GetComponent<LazoBehaviour>();
            ActivateNodeIfNeeded(lazo);
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
        
        #endregion
    }
}