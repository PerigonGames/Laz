using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Laz
{
    public class Lazo
    {
        private List<Vector3> _listOfPositions = new List<Vector3>();
        private ILazoProperties _lazoProperties = null;

        private bool _isLazoing = false;
        private float _lifeTimePerPointTimerElapsed = 0;
        private float _rateOfRecordingTimerElapsed = 0;

        public bool IsLazoing
        {
            get => _isLazoing;
            set
            {
                _isLazoing = value;
                if (!_isLazoing)
                {
                    Clear();
                    // Debug
                    DebugClearListOfCubes();
                }
            }
        }

        public Lazo(ILazoProperties properties)
        {
            _lazoProperties = properties;
            ResetLifeTimePerPointTimerElapsed();
        }

        /// <summary>
        /// Storing new positions that player moved to if able to depending on timer
        /// </summary>
        /// <param name="position">position</param>
        public void RunLazoIfAble(Vector3 position, float deltaTime)
        {
            _rateOfRecordingTimerElapsed -= deltaTime;
            _lifeTimePerPointTimerElapsed -= deltaTime;
            if (_rateOfRecordingTimerElapsed < 0)
            {
                RunLazo(position);
                RemoveOldestPointIfNeeded();
                ResetRateOfRecordingTimeElapsed();
                // Debug
                DebugCreateCubeAt(position);
            }
        }

        private void RemoveOldestPointIfNeeded()
        {

            if (_lifeTimePerPointTimerElapsed < 0)
            {
                RemoveOldestPosition();
                // Debug
                DebugDestroyLastCubeOnList();
            }
        }


        #region Helper
        private void RunLazo(Vector3 position)
        {
            if (_listOfPositions.Count > 0 &&
                _listOfPositions.Last() == position)
            {
                return;
            }

            _listOfPositions.Add(position);
        }

        /// <summary>
        /// Resets all values
        /// </summary>
        private void Clear()
        {
            _rateOfRecordingTimerElapsed = 0;
            _listOfPositions.Clear();
            ResetLifeTimePerPointTimerElapsed();
        }

        /// <summary>
        /// Remove oldest position
        /// </summary>
        private void RemoveOldestPosition()
        {
            if (_listOfPositions.Count > 0)
            {
                _listOfPositions.RemoveAt(0);
            }
        }

        private void ResetLifeTimePerPointTimerElapsed()
        {
            _lifeTimePerPointTimerElapsed = _lazoProperties.LifeTimePerPoint;
        }

        private void ResetRateOfRecordingTimeElapsed()
        {
            _rateOfRecordingTimerElapsed = _lazoProperties.RateOfRecordingPosition;
        }
        #endregion

        #region Debug
        /*
         * DEBUG
         */
        private List<GameObject> _listOfDebugCubes = new List<GameObject>();
        private void DebugCreateCubeAt(Vector3 position)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var temp = GameObject.Instantiate(cube, position, Quaternion.identity);
            _listOfDebugCubes.Add(temp);
        }

        private void DebugDestroyLastCubeOnList()
        {
            Debug.Log("Destroy Cube");
            var cube = _listOfDebugCubes[0];
            _listOfDebugCubes.RemoveAt(0);
            GameObject.Destroy(cube);
        }

        private void DebugClearListOfCubes()
        {
            if (_listOfDebugCubes.Count > 0)
            {
                foreach (var cube in _listOfDebugCubes)
                {
                    GameObject.Destroy(cube);
                }

                _listOfDebugCubes.Clear();
            }
        }

        #endregion
    }
}