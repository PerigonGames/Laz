using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Laz
{
    public class AIChomperAgro
    {
        // Dependencies
        private const int MINIMUM_AMOUNT_OF_POINTS_IN_PATH = 2;
        
        private readonly IAstarAI _ai;
        private readonly Lazo _lazo = null;
        private readonly float _extraDistance = 0;

        private int _positionIndex = 0;
        private List<Vector3> _tempLazoPositions = new List<Vector3>();
        
        private bool CanCreateCopyOfLazoPositions => _tempLazoPositions.Count <= _lazo.GetListOfLazoPositions.Count;
        
        public AIChomperAgro(IAstarAI ai, Lazo lazo, float extraDistance = 5f)
        {
            _ai = ai;
            _lazo = lazo;
            _lazo.OnLazoDeactivated += HandleOnLazoDeactivated;
            _extraDistance = extraDistance;
        }

        public void StartAgroAt(LazoPosition lazoPosition)
        {
            _ai.canSearch = false;
            _lazo.IsTimeToLiveFrozen = true;
            _positionIndex = _lazo.GetListOfLazoPositions.IndexOf(lazoPosition);
            if (lazoPosition != null)
            {
                _ai.destination = lazoPosition.Position;
            }
        }

        public void OnAgroUpdate()
        {
            if (_ai.reachedEndOfPath && !_ai.pathPending)
            {
                CopyLazoPositionsToTempLazoPositionsIfPossible();
                var listOfPositions = CreateListOfPositionsStartingFrom(_positionIndex);
                SetAIPathWith(listOfPositions);
            }
        }
        
        public void Reset()
        {
            _ai.canSearch = true;
            _tempLazoPositions = new List<Vector3>();
            _positionIndex = 0;
        }

        private void HandleOnLazoDeactivated()
        {
            CopyLazoPositionsToTempLazoPositionsIfPossible();
            var extraLastPosition = CreateExtraLastPositionForAI();
            _tempLazoPositions.Add(extraLastPosition);
        }

        private Vector3 CreateExtraLastPositionForAI()
        {
            var amountOfPositions = _tempLazoPositions.Count;
            var lastPosition = _tempLazoPositions[amountOfPositions - 1];
            var secondLastPosition = _tempLazoPositions[amountOfPositions - 2];
            var direction = NormalizedDirectionFromTwoPoints(lastPosition, secondLastPosition);
            return lastPosition + (direction * _extraDistance);
        }

        private Vector3 NormalizedDirectionFromTwoPoints(Vector3 secondLastPosition, Vector3 lastPosition)
        {
            return (secondLastPosition - lastPosition).normalized;
        }
        
        private void CopyLazoPositionsToTempLazoPositionsIfPossible()
        {
            if (CanCreateCopyOfLazoPositions)
            {
                var length = _lazo.GetListOfLazoPositions.Count;
                Vector3[] tempLazoPositionContainer = new Vector3[length];
                var listOfPositions = _lazo.GetListOfLazoPositions.Select(lazo => lazo.Position).ToList();
                listOfPositions.CopyTo(tempLazoPositionContainer);
                _tempLazoPositions = tempLazoPositionContainer.ToList();
            }
        }

        private List<Vector3> CreateListOfPositionsStartingFrom(int startingPosition)
        {
            var rangeOfPositions = _tempLazoPositions.Count - startingPosition;
            _positionIndex = _tempLazoPositions.Count - 1;
            if (rangeOfPositions > 0 && startingPosition >= 0)
            {
                return _tempLazoPositions.GetRange(startingPosition, rangeOfPositions);
            }
            
            return new List<Vector3>();
        }

        private void SetAIPathWith(List<Vector3> listOfPositions)
        {
            if (listOfPositions.Count > MINIMUM_AMOUNT_OF_POINTS_IN_PATH)
            {
                var aiPath = ABPath.FakePath(listOfPositions);
                _ai.SetPath(aiPath);
            }
        }
    }
}