using DG.Tweening;
using PerigonGames;
using UnityEngine;

namespace Laz
{
    [RequireComponent(typeof(LineRenderer))]
    public class FakeLazoBehaviour : MonoBehaviour
    {
        private FakeLazo _fakeLazo = null;
        private LineRenderer _lineRenderer = null;
        [SerializeField]
        private LazoColorPropertiesScriptableObject _lazoColors = null;

        public void SetLine(FakeLazo fakeLazo)
        {
            if (fakeLazo.IsTimeToLiveFrozen)
            {
                SetLineFrozen();
            }
            else
            {
                SetLineNormal();
            }
            HandleOnListOfLazoPositionsChanged(fakeLazo.Positions);
            _fakeLazo = fakeLazo;
            _fakeLazo.OnListOfLazoPositionsChanged += HandleOnListOfLazoPositionsChanged;
            fakeLazo.OnTimeToLiveStateChanged += HandleTimeToLiveStateChange;
        }

        private void SetLineFrozen()
        {
            _lineRenderer.colorGradient = _lazoColors.FrozenColor;
        }

        private void SetLineNormal()
        {
            _lineRenderer.colorGradient = _lazoColors.NormalGradient;
        }
        
        #region Delegate
        private void HandleOnListOfLazoPositionsChanged(Vector3[] positions)
        {
            _lineRenderer.positionCount = positions.Length;
            _lineRenderer.SetPositions(positions);
        }
        
        private void HandleTimeToLiveStateChange(bool isFrozen)
        {
            _lineRenderer.colorGradient = isFrozen ? _lazoColors.FrozenColor : _lazoColors.NormalGradient;
        }
        #endregion
        
        private void CleanUp()
        {
            if (_fakeLazo != null)
            {
                _fakeLazo.OnListOfLazoPositionsChanged -= HandleOnListOfLazoPositionsChanged;
                _fakeLazo = null;
            }
        }

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            _fakeLazo.RemoveOldestPointIfNeeded(Time.deltaTime);
        }

        private void OnDisable()
        {
            CleanUp();
        }

        private void OnDestroy()
        {
            CleanUp();
        }
    }
}
