using System;
using System.Collections.Generic;
using System.Linq;
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
            // Set the color as blue
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
            _fakeLazo.OnListOfLazoPositionsChanged -= HandleOnListOfLazoPositionsChanged;
            _fakeLazo = null;
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

    public class FakeLazo
    {
        private List<LazoPosition> _listOfPositions;
        
        public event Action<Vector3[]> OnListOfLazoPositionsChanged;
        public Vector3[] Positions { get; }

        public FakeLazo(List<LazoPosition> listOfPoints)
        {
            _listOfPositions = listOfPoints;
            Positions = _listOfPositions.Select(point => point.Position).ToArray();
        }
        
        private void RemoveOldestPointIfNeeded(float deltaTime)
        {
            _listOfPositions = _listOfPositions.Select(lazoPosition =>
            {
                lazoPosition.DecrementTimeToLiveBy(deltaTime);
                return lazoPosition;
            }).Where(lazoPosition => !lazoPosition.IsTimeBelowZero).ToList();
            OnLazoPositionsChanged();
        }
        
        private void OnLazoPositionsChanged()
        {
            if (OnListOfLazoPositionsChanged != null)
            {
                OnListOfLazoPositionsChanged(_listOfPositions.Select(x => x.Position).Reverse().ToArray());
            }
        }
    }
    
    
}
