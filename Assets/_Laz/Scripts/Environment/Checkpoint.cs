using System;
using UnityEngine;

namespace Laz
{
    [RequireComponent(typeof(Collider))]
    public class Checkpoint : MonoBehaviour
    {
        private bool _isActiveCheckpoint = false;

        public event Action<Checkpoint> OnCheckpointActivation = null;

        public bool IsActiveCheckpoint
        {
            set => _isActiveCheckpoint = value;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("New Checkpoint");
            if (!_isActiveCheckpoint && other.CompareTag(Tags.LazPlayer))
            {
                OnCheckpointActivation?.Invoke(this);
            }
        }
        
        //For visualization
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            BoxCollider collider = GetComponent<BoxCollider>();
            Gizmos.DrawCube(transform.position, collider.size);
        }
#endif
    }
}
