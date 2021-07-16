using UnityEngine;

namespace Laz
{
    [RequireComponent(typeof(Collider))]
    public class NodeConnectionBehaviour : MonoBehaviour
    {
        //Place holder Design/Feedback to show which nodes can be activated
        private const float CAN_ACTIVATE_NODE_SIZE = 0.75f;
        private const float CANNOT_ACTIVATE_NODE_SIZE = 0.3f;
        
        [SerializeField] private ParticleSystem _particleSystem = null;
        [SerializeField] private Transform _meshTransform = null;
        
        private Node _node = null;
        private LazoWallBehaviour _lazoWall = null;
        
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
            _node.OnNodeCompleted -= HandleOnNodeCompleted;
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
        
        private void ActivateNodeIfNeeded(LazoWallBehaviour lazoWall)
        {
            if (lazoWall != null && lazoWall.gameObject.activeSelf && _node.CanActivate)
            {
                _lazoWall = lazoWall;
                if (!_particleSystem.isPlaying)
                {
                    _particleSystem.Play();
                }

                _node.IsActive = true;
            }
        }
        
        #region Delegate
        private void HandleOnCanActivateChanged(bool canActivate)
        {
            if (canActivate)
            {
                _meshTransform.localScale = Vector3.one * CAN_ACTIVATE_NODE_SIZE;
            }
            else
            {
                _meshTransform.localScale = Vector3.one * CANNOT_ACTIVATE_NODE_SIZE;
            }
        }

        private void HandleOnNodeCompleted()
        {
            _meshTransform.localScale = Vector3.zero;
            ClearParticleSystem();
            _lazoWall = null;
        }
        #endregion
        
        #region Mono
        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            var lazoWall = other.GetComponent<LazoWallBehaviour>();
            ActivateNodeIfNeeded(lazoWall);
        }
        
        private void OnTriggerStay(Collider other)
        {
            var lazoWall = other.GetComponent<LazoWallBehaviour>();
            ActivateNodeIfNeeded(lazoWall);
        }
        
        private void Update()
        {
            if (_lazoWall != null && !_lazoWall.gameObject.activeSelf)
            {
                ClearParticleSystem();
                _node.IsActive = false;
                _lazoWall = null;
            }
        }
        
        #endregion
    }
}