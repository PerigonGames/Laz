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
            _node.OnCanActivateChanged += HandleOnOnCanActivateChanged;
        }

        private void CleanUp()
        {
            ClearParticleSystem();
            _node.OnCanActivateChanged -= HandleOnOnCanActivateChanged;
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

        private void HandleOnOnCanActivateChanged(bool canActivate)
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

        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            var lazo = other.GetComponent<LazoBehaviour>();
            if (lazo != null && lazo.IsLazoing)
            {
                _lazo = lazo;
                _particleSystem.Play();
            }
        }

        private void Update()
        {
            if (_lazo != null && !_lazo.IsLazoing)
            {
                ClearParticleSystem();
                _lazo = null;
            }
        }
    }
}