using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laz
{
    [RequireComponent(typeof(BoxCollider))]
    public class ElectricGate : MonoBehaviour
    {

        [SerializeField, Range(0f, 50f)] float _gateWidth = 10f;
        [SerializeField] Transform _leftPost, _rightPost, _gate = default;
        

        private BoxCollider _gateCollider = null;

        private void Awake()
        {
            _gateCollider = GetComponent<BoxCollider>();
            ScaleGate();
        }

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

        /// <summary>
        /// Resizes gate according to gate width, and repositions posts
        /// </summary>
        private void ScaleGate()
        {
            if(_gate != null)
            {
                var scale = _gate.transform.localScale;
                scale.z = _gateWidth;
                _gate.transform.localScale = scale;
            }
            if(_gateCollider != null)
            {
                var size = _gateCollider.size;
                size.z = _gateWidth;
                _gateCollider.size = size;
            }
            if(_rightPost != null)
            {
                var scale = _rightPost.localScale;
                var position = _rightPost.localPosition;
                position.z = _gateWidth * .5f;
                position.z += scale.z * .5f;
                _rightPost.localPosition = position;
            }
            if (_leftPost != null)
            {
                var scale = _leftPost.localScale;
                var position = _leftPost.localPosition;
                position.z = _gateWidth * -.5f;
                position.z -= scale.z * .5f;
                _leftPost.localPosition = position;
            }
        }

        private void OnValidate()
        {
            if (_gateCollider == null) _gateCollider = GetComponent<BoxCollider>();
            ScaleGate();
        }
    }
}