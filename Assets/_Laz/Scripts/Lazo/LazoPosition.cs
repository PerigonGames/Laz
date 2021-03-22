using UnityEngine;

namespace Laz
{
    public struct LazoPosition
    {
        public float TimeToLive;
        public Vector3 Position;

        public LazoPosition(float timeToLive, Vector3 position)
        {
            TimeToLive = timeToLive;
            Position = position;
        }
    }
}

