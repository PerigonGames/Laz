using System.Collections.Generic;
using UnityEngine;

namespace Laz
{
    public class CheckPointManager : MonoBehaviour
    {
        [SerializeField] private List<Transform> _checkPoints = null;
        public Transform LastCheckPoint { get; private set; } = null;

        public void Initialize()
        {
            UpdateCheckPoint();
        }

        //TODO - This is subject to change, data structure used can/will change depending on how checkpoints are chosen
        public void UpdateCheckPoint()
        {
            if (_checkPoints != null && _checkPoints.Count > 0)
            {
                LastCheckPoint = _checkPoints[0];
                _checkPoints.RemoveAt(0);
            }
        }

    }
}

