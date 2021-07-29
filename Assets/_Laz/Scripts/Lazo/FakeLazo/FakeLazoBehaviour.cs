using PerigonGames;
using Sirenix.OdinInspector;
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
            SetLineFrozen();
            HandleOnListOfLazoPositionsChanged(fakeLazo.Positions);
            _fakeLazo = fakeLazo;
            _fakeLazo.OnListOfLazoPositionsChanged += HandleOnListOfLazoPositionsChanged;
        }

        [Button]
        private void SetLineFrozen()
        {
            _lineRenderer.colorGradient = _lazoColors.FrozenColor;
        }

        [Button]
        private void SetLineNormal()
        {
            _lineRenderer.colorGradient = _lazoColors.NormalGradient;
        }
        
        private void HandleOnListOfLazoPositionsChanged(Vector3[] positions)
        {
            if (positions.IsNullOrEmpty())
            {
                gameObject.SetActive(false);
            }
            
            _lineRenderer.positionCount = positions.Length;
            _lineRenderer.SetPositions(positions);
        }

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
