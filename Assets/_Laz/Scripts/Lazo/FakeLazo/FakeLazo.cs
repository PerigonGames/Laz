using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Laz
{
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