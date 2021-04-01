using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Laz
{
    public class Lazo
    {
        private List<LazoPosition> _listOfPositions = new List<LazoPosition>();
        private ILazoProperties _lazoProperties = null;

        private bool _isDebugging = false;
        private bool _isLazoing = false;
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

        public Lazo(ILazoProperties properties, bool isDebugging)
        {
            _lazoProperties = properties;
            _isDebugging = isDebugging;
        }

        /// <summary>
        /// Storing new positions that player moved to if able to depending on timer
        /// </summary>
        /// <param name="position">position</param>
        public void RunLazoIfAble(Vector3 position, float deltaTime)
        {
            _rateOfRecordingTimerElapsed -= deltaTime;
            if (_rateOfRecordingTimerElapsed < 0)
            {
                RunLazo(position);
                ResetRateOfRecordingTimeElapsed();
            }

            RemoveOldestPointIfNeeded(deltaTime);
        }

        private void RemoveOldestPointIfNeeded(float deltaTime)
        {
            _listOfPositions = _listOfPositions.Select(lazoPosition =>
            {
                lazoPosition.TimeToLive -= deltaTime;
                // Debug
                if (lazoPosition.TimeToLive < 0)
                {
                    DebugDestroyLastCubeOnList();
                }
                return lazoPosition;
            }).Where(lazoPosition => lazoPosition.TimeToLive > 0).ToList();
        }


        #region Helper
        private void RunLazo(Vector3 position)
        {
            if (_listOfPositions.Count > 0 &&
                _listOfPositions.Last().Position == position)
            {
                return;
            }

            Debug.Log("Number of Position: " + _listOfPositions.Count);
            var lazoPosition = new LazoPosition(_lazoProperties.TimeToLivePerPoint, position);
            _listOfPositions.Add(lazoPosition);

            // Debug
            DebugCreateCubeAt(position);
        }

        /// <summary>
        /// Resets all values
        /// </summary>
        private void Clear()
        {
            _rateOfRecordingTimerElapsed = 0;
            _listOfPositions.Clear();
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
            if (!_isDebugging) return;
            Debug.Log("Number of Cube: "+ _listOfDebugCubes.Count);
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = position;
            cube.GetComponent<BoxCollider>().enabled = false;
            _listOfDebugCubes.Add(cube);
        }

        private void DebugDestroyLastCubeOnList()
        {
            if (!_isDebugging) return;
            if (_listOfDebugCubes.Count > 0)
            {
                var cube = _listOfDebugCubes[0];
                _listOfDebugCubes.RemoveAt(0);
                GameObject.Destroy(cube);
            }
        }

        private void DebugClearListOfCubes()
        {
            if (!_isDebugging) return;
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