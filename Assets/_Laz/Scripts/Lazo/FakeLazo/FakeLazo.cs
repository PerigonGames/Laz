using System;
using System.Collections.Generic;
using System.Linq;
using PerigonGames;
using UnityEngine;

namespace Laz
{
    public class FakeLazo
    {
        private const float RATE_OF_SHRINK = 0.01F;
        private List<LazoPosition> _listOfPositions;
        private bool _isTimeToLiveFrozen = false;
        private float _shrinkTime = 0f;
        private List<AIChomperAgro> _listOfAgroChompers = new List<AIChomperAgro>();
        private bool _canStartShrinking = false;
        
        
        public event Action<Vector3[]> OnListOfLazoPositionsChanged;
        public event Action<bool> OnTimeToLiveStateChanged;
        public Vector3[] Positions { get; }

        public bool IsTimeToLiveFrozen
        {
            get => _isTimeToLiveFrozen;
            private set
            {
                _isTimeToLiveFrozen = value;
                OnTimeToLiveStateChanged?.Invoke(value);
            }
        }

        public FakeLazo(List<LazoPosition> listOfPoints, bool isFrozen)
        {
            _canStartShrinking = false;
            _shrinkTime = RATE_OF_SHRINK;
            _isTimeToLiveFrozen = isFrozen;
            DeepCopy(listOfPoints);
            Positions = _listOfPositions.Select(point => point.Position).ToArray();
        }

        private void DeepCopy(List<LazoPosition> listOfPoints)
        {
            var tempCopy = new List<LazoPosition>();
            foreach(var point in listOfPoints)
            {
                var tempLazoPosition = new LazoPosition(point.TimeToLive, point.Position);
                tempCopy.Add(tempLazoPosition);
            }

            _listOfPositions = tempCopy;
        }

        public void CleanUp()
        {
            _listOfAgroChompers = null;
            _isTimeToLiveFrozen = false;
            _listOfPositions.ForEach(point => point.ForceDeath());
            OnLazoPositionsChanged();
        }

        public void Reset()
        {
            _listOfAgroChompers = new List<AIChomperAgro>();
            _listOfPositions = new List<LazoPosition>();
            _shrinkTime = RATE_OF_SHRINK;
            _canStartShrinking = false;
        }

        public void RemoveOldestPointIfNeeded(float deltaTime)
        {
            if (_listOfAgroChompers.Count > 0 || !_canStartShrinking)
            {
                return;
            }
            
            _shrinkTime -= deltaTime;
            if (_shrinkTime < 0)
            {
                _shrinkTime = RATE_OF_SHRINK;
                KillAndRemoveFirstLazoPosition();
            }
        }

        public void AddChomperToList(AIChomperAgro chomper)
        {            
            _canStartShrinking = true;
            _listOfAgroChompers.Add(chomper);
        }

        public void RemoveChomperFromList(AIChomperAgro chomper)
        {
            _listOfAgroChompers.Remove(chomper);
        }

        private void KillAndRemoveFirstLazoPosition()
        {
            if (!_listOfPositions.IsNullOrEmpty())
            {
                _listOfPositions.First().ForceDeath();
                _listOfPositions = _listOfPositions.Where(lazoPosition => !lazoPosition.IsTimeBelowZero).ToList();
                OnLazoPositionsChanged();
            }
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