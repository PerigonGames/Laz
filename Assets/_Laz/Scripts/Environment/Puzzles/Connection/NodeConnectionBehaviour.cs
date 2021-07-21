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
        private LazoBehaviour _lazoBehaviour = null;

        private bool CanActivateNode => _node.CanActivate && !_node.IsActive;
        
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
        
        private void ActivateNodeIfNeeded(LazoBehaviour lazoBehaviour)
        {
            if (lazoBehaviour is {IsLazoing: true} && CanActivateNode)
            {
                _lazoBehaviour = lazoBehaviour;
                _lazoBehaviour.LazoModel.OnLoopClosed += HandleOnLazoLoopClosed;
                
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
            _lazoBehaviour = null;
        }
        
        private void HandleOnLazoLoopClosed(LazoPosition[] positions)
        {
            if (_lazoBehaviour != null)
            {
                DeactivateNode();
            }
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
            if (_lazoBehaviour is { IsLazoing: false })
            {
                DeactivateNode();
            }
        }

        private void DeactivateNode()
        {
            ClearParticleSystem();
            _node.IsActive = false;
            _lazoBehaviour.LazoModel.OnLoopClosed -= HandleOnLazoLoopClosed;
            _lazoBehaviour = null;
        }
        #endregion
    }
}