using UnityEngine;

namespace Laz
{
    public interface IAIDetectionDataSource
    { 
        void RayCastDidCollideWith(GameObject collidedGameObject);
    }
    
    public class AIDetectionBehaviour : MonoBehaviour
    {
        private float _detectionRadius = 5;
        private IAIDetectionDataSource _dataSource;
        
        public void Initialize(float detectionRadius, IAIDetectionDataSource dataSource)
        {
            _detectionRadius = detectionRadius;
            _dataSource = dataSource;
        }

        public void Detect()
        {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, _detectionRadius, transform.forward, out hit))
            {
                _dataSource.RayCastDidCollideWith(hit.collider.gameObject);
            }
        }
        
#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 1, 0, 0.25f);
            Gizmos.DrawSphere(transform.position, _detectionRadius);
        }
#endif
    }
}