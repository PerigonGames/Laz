using System;
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

        public event Action OnChomperReachedEndOfLazo;
        
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
            _tempLazoPositions = new List<Vector3>();
            _ai.canSearch = false;
            _lazo.IsTimeToLiveFrozen = true;
            _positionIndex = _lazo.GetListOfLazoPositions.IndexOf(lazoPosition);
            if (lazoPosition != null)
            {
                _ai.destination = lazoPosition.Position;
                _ai.SearchPath();
            }
        }


        public void OnAgroUpdate()
        {
            if (_ai.reachedEndOfPath && !_ai.pathPending)
            {
                CopyLazoPositionsToTempLazoPositionsIfPossible();
                var listOfPositions = CreateListOfPositionsStartingFrom(_positionIndex);
                SetAIPathWith(listOfPositions);
                if (!CanStillCreateFakePath(listOfPositions))
                {
                    _ai.canSearch = true;
                    OnChomperReachedEndOfLazo?.Invoke();
                }
            }
        }

        public void CleanUp()
        {
            _tempLazoPositions = null;
            _lazo.OnLazoDeactivated -= HandleOnLazoDeactivated;
        }
        
        public void Reset()
        {
            _ai.canSearch = true;
            _tempLazoPositions = new List<Vector3>();
            _positionIndex = 0;
            _lazo.OnLazoDeactivated += HandleOnLazoDeactivated;
        }

        #region delegate
        private void HandleOnLazoDeactivated()
        {
            CopyLazoPositionsToTempLazoPositionsIfPossible();
            var extraLastPosition = CreateExtraLastPositionForAI();
            _tempLazoPositions.Add(extraLastPosition);
        }
#endregion

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

        private List<Vector3> CreateListOfPositionsStartingFrom(int fromPosition)
        {
            var rangeOfPositions = _tempLazoPositions.Count - fromPosition;
            _positionIndex = _tempLazoPositions.Count - 1;
            if (rangeOfPositions > 0 && fromPosition >= 0)
            {
                return _tempLazoPositions.GetRange(fromPosition, rangeOfPositions);
            }
            
            return new List<Vector3>();
        }

        private void SetAIPathWith(List<Vector3> listOfPositions)
        {
            if (CanStillCreateFakePath(listOfPositions))
            {
                var aiPath = ABPath.FakePath(listOfPositions);
                _ai.SetPath(aiPath);
            }
        }

        private bool CanStillCreateFakePath(List<Vector3> listOfPositions)
        {
            return listOfPositions.Count > MINIMUM_AMOUNT_OF_POINTS_IN_PATH;
        }
    }
}