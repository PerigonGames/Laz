using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using PerigonGames;
using UnityEngine;

namespace Laz
{
    public class AIChomperAgroBehaviour : MonoBehaviour
    {
        // Dependencies
        private IAstarAI _ai;
        private int _positionIndex = 0;
        private Lazo _lazo = null;
        private float _extraDistance = 0;

        private List<Vector3> _tempLazoPositions = new List<Vector3>();
        
        public void Initialize(IAstarAI ai, Lazo lazo, float extraDistance = 5f)
        {
            _ai = ai;
            _lazo = lazo;
            _lazo.OnLazoDeactivated += HandleOnLazoDeactivated;
            _extraDistance = extraDistance;
        }

        public void StartAgroAt(LazoPosition lazoPosition)
        {
            _lazo.IsTimeToLiveFrozen = true;
            _positionIndex = _lazo.GetListOfLazoPositions.IndexOf(lazoPosition);
            if (lazoPosition != null)
            {
                _ai.destination = lazoPosition.Position;
            }
        }

        public void Agro()
        {
            if (_ai.reachedEndOfPath && !_ai.pathPending)
            {
                _ai.canSearch = false;
                CopyLazoPositionsToTempLazoPositions();
                var listOfPositions = CreateListOfPositionsStartingFrom(_positionIndex);
                if (listOfPositions.IsNullOrEmpty())
                {
                    // Go a bit further
                }
                SetAIPathWith(listOfPositions);
            }
        }

        private void HandleOnLazoDeactivated()
        {
            CopyLazoPositionsToTempLazoPositions();
            
            var length = _tempLazoPositions.Count;
            var last = _tempLazoPositions[length - 1];
            var secondLast = _tempLazoPositions[length - 2];
            var direction = NormalizedDirectionFromTwoPoints(last, secondLast);
            var lastPosition = _tempLazoPositions[length - 1] + (direction * _extraDistance);
            
            _tempLazoPositions.Add(lastPosition);
        }

        private Vector3 NormalizedDirectionFromTwoPoints(Vector3 secondLastPosition, Vector3 lastPosition)
        {
            return (secondLastPosition - lastPosition).normalized;
        }
        
        
        private void CopyLazoPositionsToTempLazoPositions()
        {
            if (_tempLazoPositions.Count <= _lazo.GetListOfLazoPositions.Count)
            {
                var length = _lazo.GetListOfLazoPositions.Count;
                Vector3[] temp = new Vector3[length];
                var listOfPositions = _lazo.GetListOfLazoPositions.Select(lazo => lazo.Position).ToList();
                listOfPositions.CopyTo(temp);
                _tempLazoPositions = temp.ToList();
            }
        }

        private List<Vector3> CreateListOfPositionsStartingFrom(int startingPosition)
        {
            var rangeOfPositions = _tempLazoPositions.Count - startingPosition;
            _positionIndex = _tempLazoPositions.Count - 1;
            if (rangeOfPositions > 0 &&  startingPosition > 0)
            {
                return _tempLazoPositions.GetRange(startingPosition, rangeOfPositions);
            }

            return new List<Vector3>();
        }

        private void SetAIPathWith(List<Vector3> listOfPositions)
        {
            if (listOfPositions.Count > 2)
            {
                var aiPath = ABPath.FakePath(listOfPositions);
                _ai.SetPath(aiPath);
            }
        }
        
        #region Mono

        private void OnDestroy()
        {
            _lazo.OnLazoDeactivated -= CopyLazoPositionsToTempLazoPositions;
        }

        #endregion
    }

    public class AIChomperAgro
    {
        
    }
}