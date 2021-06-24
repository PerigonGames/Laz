using System.Collections.Generic;
using UnityEngine;

namespace Laz
{
    public interface IPatrolBehaviour
    {
        public List<Vector3> PatrolPositions
        {
            get;
            set;
        }
    }
}
