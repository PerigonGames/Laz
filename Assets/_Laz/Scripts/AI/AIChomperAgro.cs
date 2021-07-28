using System;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
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

        private FakeLazo _fakeLazo = null;
        private int _positionIndex = 0;
        private List<Vector3> _tempLazoPositions = new List<Vector3>();
        private bool HasFrozenLazo => _lazo.IsTimeToLiveFrozen;

        public event Action OnChomperReachedEndOfLazo;
        
        private bool CanCreateCopyOfLazoPositions => _tempLazoPositions.Count <= _lazo.GetListOfLazoPositions.Count;
        
        public AIChomperAgro(IAstarAI ai, Lazo lazo, float extraDistance = 5f)
        {
            _ai = ai;
            _lazo = lazo;
            _extraDistance = extraDistance;
        }

        public void StartAgroAt(LazoPosition lazoPosition)
        {
            _lazo.OnLazoDeactivated += HandleOnLazoDeactivated;
            _tempLazoPositions = new List<Vector3>();
            _ai.canSearch = false;
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
                FreezeLazoIfNeeded();
                CopyLazoPositionsToTempLazoPositionsIfPossible();
                var listOfPositions = CreateListOfPositionsStartingFrom(_positionIndex);
                SetAIPathWith(listOfPositions);
                if (!CanStillCreateFakePath(listOfPositions))
                {
                    _ai.canSearch = true;
                    _fakeLazo.IsTimeToLiveFrozen = false;
                    _fakeLazo = null;
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
            _lazo.OnLazoDeactivated -= HandleOnLazoDeactivated;
        }

        #region delegate
        private void HandleOnLazoDeactivated()
        {
            CreateFakeLazoLineForChomperToRideOn();
            CopyLazoPositionsToTempLazoPositionsIfPossible();
            var extraLastPosition = CreateExtraLastPositionForAI();
            _tempLazoPositions.Add(extraLastPosition);
            _lazo.OnLazoDeactivated -= HandleOnLazoDeactivated;
        }

        private void CreateFakeLazoLineForChomperToRideOn()
        {
            var fakeLazoBehaviour = FakeLazoObjectPooler.Instance.PopInActivePooledObject(FakeLazoObjectPooler.Key);
            _fakeLazo = new FakeLazo(_lazo.GetListOfLazoPositions, _lazo.IsTimeToLiveFrozen);
            fakeLazoBehaviour.SetLine(_fakeLazo);
            fakeLazoBehaviour.gameObject.SetActive(true);
        }
        #endregion

        private void FreezeLazoIfNeeded()
        {
            if (_lazo != null && !HasFrozenLazo)
            {
                _lazo.IsTimeToLiveFrozen = true;
            }
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