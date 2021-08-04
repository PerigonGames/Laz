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

        public event Action OnChomperReachedEndOfLazo;
        
        public AIChomperAgro(IAstarAI ai, Lazo lazo, float extraDistance = 5f)
        {
            _ai = ai;
            _lazo = lazo;
            _extraDistance = extraDistance;
            _lazo.OnLazoDeactivated += HandleOnLazoDeactivated;
        }

        public void SetLazoPosition(LazoPosition lazoPosition)
        {
            _positionIndex = _lazo.GetListOfLazoPositions.IndexOf(lazoPosition);
        }

        public void StartAgroAt()
        {
            _ai.canSearch = false;
            if (_fakeLazo == null)
            {
                CopyLazoPositionsToTempLazoPositions();
            }
            
            var listOfPositions = CreateListOfPositionsStartingFrom(_positionIndex);
            SetAIPathWith(listOfPositions);
        }
        
        public void OnAgroUpdate()
        {                
            if (_ai.reachedEndOfPath && !_ai.pathPending)
            {
                if (_fakeLazo == null)
                {
                    CopyLazoPositionsToTempLazoPositions();
                }
                
                var listOfPositions = CreateListOfPositionsStartingFrom(_positionIndex);
                SetAIPathWith(listOfPositions);


                if (!CanStillCreateFakePath(listOfPositions))
                {
                    _ai.canSearch = true;
                    if (_fakeLazo != null)
                    {
                        _fakeLazo.IsTimeToLiveFrozen = false;
                        _fakeLazo = null;
                    }
                    _positionIndex = 0;
                    _tempLazoPositions.Clear();
                    OnChomperReachedEndOfLazo?.Invoke();
                }
            }
        }

        public void CleanUp()
        {
            if (_fakeLazo != null)
            {
                _fakeLazo.CleanUp();
                _fakeLazo = null;
            }
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
            if (_lazo.IsTimeToLiveFrozen)
            {
                CreateFakeLazoLineForChomperToRideOn();
                CopyLazoPositionsToTempLazoPositions();
                var extraLastPosition = CreateExtraLastPositionForAI();
                if (extraLastPosition != null)
                {
                    _tempLazoPositions.Add((Vector3) extraLastPosition);
                }
                _lazo.IsTimeToLiveFrozen = false;
            }
        }

        private void CreateFakeLazoLineForChomperToRideOn()
        {
            var fakeLazoBehaviour = FakeLazoObjectPooler.Instance.PopInActivePooledObject(FakeLazoObjectPooler.Key);
            _fakeLazo = new FakeLazo(_lazo.GetListOfLazoPositions, _lazo.IsTimeToLiveFrozen);
            fakeLazoBehaviour.SetLine(_fakeLazo);
            fakeLazoBehaviour.gameObject.SetActive(true);
        }
        #endregion

        private Vector3? CreateExtraLastPositionForAI()
        {
            var amountOfPositions = _tempLazoPositions.Count;
            if (amountOfPositions > 1)
            {
                var lastPosition = _tempLazoPositions[amountOfPositions - 1];
                var secondLastPosition = _tempLazoPositions[amountOfPositions - 2];
                var direction = NormalizedDirectionFromTwoPoints(lastPosition, secondLastPosition);
                return lastPosition + (direction * _extraDistance);
            }

            return null;
        }

        private Vector3 NormalizedDirectionFromTwoPoints(Vector3 secondLastPosition, Vector3 lastPosition)
        {
            return (secondLastPosition - lastPosition).normalized;
        }
        
        private void CopyLazoPositionsToTempLazoPositions()
        {
            var length = _lazo.GetListOfLazoPositions.Count;
            Vector3[] tempLazoPositionContainer = new Vector3[length];
            var listOfPositions = _lazo.GetListOfLazoPositions.Select(lazo => lazo.Position).ToList();
            listOfPositions.CopyTo(tempLazoPositionContainer);
            _tempLazoPositions = tempLazoPositionContainer.ToList();
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