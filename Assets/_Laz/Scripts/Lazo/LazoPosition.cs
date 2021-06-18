using System;
using UnityEngine;

namespace Laz
{
    public class LazoPosition
    {
        public float _timeToLive;
        public Vector3 Position;

        public bool IsTimeBelowZero => _timeToLive < 0;

        public event Action OnTimeBelowZero;
        
        public LazoPosition(float timeToLive, Vector3 position)
        {
            _timeToLive = timeToLive;
            Position = position;
        }

        public void ForceDeath()
        {
            _timeToLive = -1;
            if (OnTimeBelowZero != null)
            {
                OnTimeBelowZero();
            }
        }

        public void DecrementTimeToLiveBy(float deltaTime)
        {
            _timeToLive -= deltaTime;
            if (IsTimeBelowZero && OnTimeBelowZero != null)
            {
                OnTimeBelowZero();
            }
        }

        public void TriggeredPosition()
        {
            //TODO - For future usage
        }
    }
}

