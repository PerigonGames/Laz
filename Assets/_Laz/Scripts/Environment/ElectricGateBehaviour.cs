using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Laz
{
    [RequireComponent(typeof(BoxCollider))]
    public class ElectricGateBehaviour : BaseCompletedPuzzleActivationBehaviour
    {

        private const float HALF = 0.5f;
        private const float DISABLED_ALPHA = 0.2f;

#if UNITY_EDITOR
        [SerializeField, Range(0f, 50f), Tooltip("Proxy for gate's z scale. Adjust this to increase the width of the gate")] 
        private float _gateWidth = 10f;
        [SerializeField] 
        private Transform _leftPost, _rightPost = default;
#endif
        [SerializeField] private Transform _gateTransform = default;
        [SerializeField] private Renderer _gateRenderer = null;

        [SerializeField, Tooltip("Initial state for the gate. If true, gate will start as on and puzzles will deactivate gate.")] private bool _initialState = true;

        [SerializeField, Tooltip("If true, gate will flicker on and off. If false, gate is always on")]
        private bool _flickering = false;

        [ShowIfGroup("_flickering")]
        [BoxGroup("_flickering/Flickering Settings")]
        [SerializeField] private float _onTime = 0.5f, _offTime = 0.5f;

        private BoxCollider _gateCollider = null;
        private Material _gateMaterial = null;
        private Color _gateColor;

        private ElectricGate _gate;

        public override void Initialize()
        {
            Reset();
        }

        public override void CleanUp()
        {
            _gate.OnGateFlickerChange -= HandleGateStateChange;
            _gate = null;
        }

        public override void Reset()
        {
            if (_initialState)
            {
                _gateTransform.gameObject.SetActive(true);
                _gateCollider.enabled = true;
                _gate = new ElectricGate(_onTime, _offTime, _flickering);
                _gate.OnGateFlickerChange += HandleGateStateChange;
            }
            else
            {
                _gateTransform.gameObject.SetActive(false);
                _gateCollider.enabled = false;
            }
        }



        public override void OnActivated()
        {
            if (_initialState)
            {
                _gateTransform.gameObject.SetActive(false);
                _gateCollider.enabled = false;
                CleanUp();
            }
            else
            {
                _gateTransform.gameObject.SetActive(true);
                _gateCollider.enabled = true;
                _gate = new ElectricGate(_onTime, _offTime, _flickering);
                _gate.OnGateFlickerChange += HandleGateStateChange;
            }
        }

        private void GateFlickerOff()
        {
            SetMaterialAlpha(DISABLED_ALPHA);
            _gateCollider.enabled = false;
        }

        private void GateFlickerOn()
        {
            SetMaterialAlpha(1f);
            _gateCollider.enabled = true;
        }

        private void SetMaterialAlpha(float alpha)
        {
            if (_gateMaterial == null)
            {
                _gateMaterial = _gateRenderer.material;
                _gateColor = _gateMaterial.color;
            }
            _gateColor.a = alpha;
            _gateMaterial.color = _gateColor;
        }

        #region MONOBEHAVIOUR_METHODS
        private void Awake()
        {
            _gateCollider = GetComponent<BoxCollider>();
            if (!_gateCollider) Debug.LogError($"{name} is missing a box collider!");
            if (!_gateRenderer) Debug.LogError($"{name} has an unset renderer!");
            if (!_gateTransform) Debug.LogError($"{name} needs to have the gate transform set!");
        }

        private void Update()
        {
            _gate.Update();
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
        #endregion

        #region DELEGATE
        private void HandleGateStateChange(bool gateOn)
        {
            if(gateOn)
                GateFlickerOn();
            else  
                GateFlickerOff();
        }
        #endregion

#if UNITY_EDITOR
        private void SetUpGate()
        {
            var scale = _gateTransform.transform.localScale;
            scale.z = _gateWidth;
            _gateTransform.transform.localScale = scale;

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

            if (_gateTransform == null || _gateCollider == null || _leftPost == null || _rightPost == null)
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