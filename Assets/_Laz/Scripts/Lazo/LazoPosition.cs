using System;
using UnityEngine;

namespace Laz
{
    public class LazoPosition
    {
        private float _timeToLive;
        private readonly Vector3 _position;

        public bool IsTimeBelowZero => _timeToLive < 0;
        public Vector3 Position => _position;

        public float TimeToLive
        {
            get => _timeToLive;
            set => _timeToLive = value;
        }

        public event Action OnTimeBelowZero;
        
        public LazoPosition(float timeToLive, Vector3 position)
        {
            _timeToLive = timeToLive;
            _position = position;
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

