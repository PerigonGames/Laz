using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Laz
{
    public class Lazo
    {
        private List<Vector3> _listOfPositions = new List<Vector3>();

        /// <summary>
        /// Storing new positions that player moved to
        /// </summary>
        /// <param name="position">position</param>
        public void RunLazo(Vector3 position)
        {
            if (_listOfPositions.Count > 0 &&
                _listOfPositions.Last() == position)
            {
                return;
            }

            _listOfPositions.Add(position);
        }

        /// <summary>
        /// Clears all position
        /// </summary>
        public void Clear()
        {
            _listOfPositions.Clear();
        }

        /// <summary>
        /// Remove oldest position
        /// </summary>
        public void RemoveOldestPosition()
        {
            if (_listOfPositions.Count > 0)
            {
                _listOfPositions.RemoveAt(0);
            }
        }
    }
}