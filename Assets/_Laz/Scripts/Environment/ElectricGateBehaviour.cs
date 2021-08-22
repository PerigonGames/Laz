using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laz
{
    [RequireComponent(typeof(BoxCollider))]
    public class ElectricGateBehaviour : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, Range(0f, 50f), Tooltip("Proxy for gate's z scale. Adjust this to increase the width of the gate")] 
        private float _gateWidth = 10f;
        [SerializeField] 
        private Transform _leftPost, _rightPost, _gate = default;
        
        private BoxCollider _gateCollider = null;

        private const float HALF = 0.5f;

        private void SetUpGate()
        {
            var scale = _gate.transform.localScale;
            scale.z = _gateWidth;
            _gate.transform.localScale = scale;

            var size = _gateCollider.size;
            size.z = _gateWidth;
            _gateCollider.size = size;
        }

        private void SetUpLeftPost()
        {
            var scale = _leftPost.localScale;
            var position = _leftPost.localPosition;
            position.z = _gateWidth * -HALF;
            position.z -= scale.z * HALF;
            _leftPost.localPosition = position;
        }

        private void SetUpRightPost()
        {
            var scale = _rightPost.localScale;
            var position = _rightPost.localPosition;
            position.z = _gateWidth * HALF;
            position.z += scale.z * HALF;
            _rightPost.localPosition = position;
        }

        private void OnValidate()
        {
            if (_gateCollider == null) _gateCollider = GetComponent<BoxCollider>();

            if (_gate == null || _gateCollider == null || _leftPost == null || _rightPost == null)
            {
                Debug.LogWarning("Electric gate needs to be configured");
                return;
            }

            SetUpGate();
            SetUpLeftPost();
            SetUpRightPost();
        }
#endif

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag(Tags.LazPlayer))
            {
                LazCoordinatorBehaviour lazCoordinator = other.gameObject.GetComponent<LazCoordinatorBehaviour>();

                if (lazCoordinator != null)
                {
                    lazCoordinator.KillLaz();
                }
            }
        }

    }
}