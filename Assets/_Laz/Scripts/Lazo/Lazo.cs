using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PerigonGames;
using UnityEngine;

namespace Laz
{
    public class Lazo
    {
        public event Action OnLoopClosed;
        
        private List<LazoPosition> _listOfPositions = new List<LazoPosition>();
        private ILazoProperties _lazoProperties = null;
        private IObjectOfInterest[] _objectOfInterests = null;

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

        public Lazo(ILazoProperties properties, IObjectOfInterest[] objectOfInterests, bool isDebugging = false)
        {
            _objectOfInterests = objectOfInterests ?? new IObjectOfInterest[]{};
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
        
        private void RunLazo(Vector3 position)
        {
            if (_listOfPositions.Count > 0 &&
                _listOfPositions.Last().Position == position)
            {
                return;
            }

            var lazoPosition = new LazoPosition(_lazoProperties.TimeToLivePerPoint, position);
            _listOfPositions.Add(lazoPosition);
            
            if (IsClosedLoop(out var closedOffPosition))
            {
                if (OnLoopClosed != null)
                {
                    OnLoopClosed();
                }
                
                var length = _listOfPositions.Count - closedOffPosition;
                //TODO - This can be bugged, it does not get the intersections of 2 lines(4 points). 
                var polygon = _listOfPositions.GetRange(closedOffPosition, length - 1).ToArray();
                var objectOfInterests = GetObjectOfInterestsWithin(polygon);
                if (!objectOfInterests.IsNullOrEmpty())
                {
                    foreach (var interest in objectOfInterests)
                    {
                        interest.OnLazoActivated();
                    }
                }

                Clear();
            }
            
            
            // Debug
            DebugCreateCubeAt(position);
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
        
        private bool IsClosedLoop(out int closedOffPosition)
        {
            closedOffPosition = 0;
            var length = _listOfPositions.Count();
            if (length < 4)
            {
                return false;
            }

            // Last Line
            var nMinusOnePosition = _listOfPositions[length - 2].Position;
            var lastPosition = _listOfPositions[length - 1].Position;
        
            for (var i = 0; i < length - 3; i++)
            {
                // First Line
                var firstPosition = _listOfPositions[i].Position;
                var secondPosition = _listOfPositions[i + 1].Position;

                if (GeometryUtilities.IsIntersecting(firstPosition, secondPosition, nMinusOnePosition, lastPosition))
                {
                    closedOffPosition = i;
                    return true;
                }
            }
        
            return false;
        }

        private IObjectOfInterest[] GetObjectOfInterestsWithin(LazoPosition[] polygon)
        {
            List<IObjectOfInterest> listOfObjects = new List<IObjectOfInterest>();
            Vector3[] arrayOfLazoPositions = polygon.Select(position => position.Position).ToArray();
            foreach (var piece in _objectOfInterests)
            {
                if (GeometryUtilities.IsInside(arrayOfLazoPositions, piece.Position))
                {
                    listOfObjects.Add(piece);
                }
            }
        
            return listOfObjects.ToArray();
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