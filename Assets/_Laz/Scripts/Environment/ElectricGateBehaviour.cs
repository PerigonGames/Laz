using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
#endif
        [SerializeField] private Renderer _gateRenderer = null;

        [SerializeField, Tooltip("If true, gate will flicker on and off. If false, gate is always on")]
        private bool _flickering = false;

        [ShowIfGroup("_flickering")]
        [BoxGroup("_flickering/Flickering Settings")]
        [SerializeField] private float _onTime = 0.5f, _offTime = 0.5f;

        private BoxCollider _gateCollider = null;
        private Material _gateMat = null;
        private Color _gateColor;

        private bool _flickeringState = true;

        private float _time = 0.0f;
        private const float HALF = 0.5f;
        private const float DISABLED_ALPHA = 0.2f;

        private void Awake()
        {
            _gateCollider = GetComponent<BoxCollider>();
            if (!_gateCollider) Debug.LogError($"{name} is missing a box collider!");
            if (!_gateRenderer) Debug.LogError($"{name} has an unset renderer!");
        }

        private void Update()
        {
            if (_flickering)
            {
                _time += Time.deltaTime;
                if(_flickeringState && _time > _onTime)
                {
                    GateFlickerOff();
                }
                else if(!_flickeringState && _time > _offTime)
                {
                    GateFlickerOn();
                }
            }
        }

        private void GateFlickerOff()
        {
            _time -= _onTime;
            _flickeringState = false;
            SetMaterialAlpha(DISABLED_ALPHA);
            _gateCollider.enabled = false;
        }

        private void GateFlickerOn()
        {
            _time -= _offTime;
            _flickeringState = true;
            SetMaterialAlpha(1f);
            _gateCollider.enabled = true;
        }

        private void SetMaterialAlpha(float alpha)
        {
            if (_gateMat == null)
            {
                _gateMat = _gateRenderer.material;
                _gateColor = _gateMat.color;
            }
            _gateColor.a = alpha;
            _gateMat.color = _gateColor;
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

#if UNITY_EDITOR
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
    }
}