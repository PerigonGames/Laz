using System;
using System.Collections.Generic;
using System.Linq;
using PerigonGames;
using UnityEngine;

namespace Laz
{
    public class Lazo
    {
        private List<LazoPosition> _listOfPositions = new List<LazoPosition>();
        private ILazoProperties _lazoProperties = null;
        private ILazoWrapped[] _objectOfInterests = null;

        private bool _isLazoing = false;
        private float _rateOfRecordingTimerElapsed = 0;
        private float _travelledDistance = 0;
        private Vector3? _lastPosition = null;
        
        public event Action<LazoPosition[]> OnLoopClosed;
        public event Action OnLazoLimitReached;
        
        public float CoolDown => _lazoProperties.CoolDown;
        public float TimeToLivePerPoint => _lazoProperties.TimeToLivePerPoint;

        private float TravelledDistance
        {
            get => _travelledDistance;
            set
            {
                _travelledDistance = value;
                if (OnLazoLimitChanged != null)
                {
                    OnLazoLimitChanged(_travelledDistance / _lazoProperties.DistanceLimitOfLazo);
                }
            }
        }

        public event Action<float> OnLazoLimitChanged; 
        
        public bool IsLazoing
        {
            get => _isLazoing;
            set
            {
                _isLazoing = value;
                if (!_isLazoing)
                {
                    Reset();
                    // Debug
                    DebugClearListOfCubes();
                }
            }
        }

        public Lazo(ILazoProperties properties, ILazoWrapped[] objectOfInterests)
        {
            _objectOfInterests = objectOfInterests ?? new ILazoWrapped[]{};
            _lazoProperties = properties;
            Reset();
        }

        public void CleanUp()
        {
            _isLazoing = false;
            _rateOfRecordingTimerElapsed = 0;
            _travelledDistance = 0;
            _lastPosition = null;
            DebugClearListOfCubes();
        }

        public void Reset()
        {
            _isLazoing = false;
            _rateOfRecordingTimerElapsed = 0;
            _listOfPositions.Clear();
            ResetTravelledDistance();
            _lastPosition = null;
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

        public void DidLazoLimitReached(Vector3 position)
        {
            if (_lastPosition == null)
            {
                _lastPosition = position;
                return;
            }
            
            TravelledDistance -= DistanceBetweenV3ToV2((Vector3)_lastPosition, position);
            _lastPosition = position;
            
            if (TravelledDistance <= 0)
            {
                IsLazoing = false;
                _lastPosition = null;
                if (OnLazoLimitReached != null)
                {
                    OnLazoLimitReached();
                }
            }
        }

        public void ResetTravelledDistance()
        {
            TravelledDistance = _lazoProperties.DistanceLimitOfLazo;
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
                var polygon = GetPolygon(closedOffPosition);
                if (OnLoopClosed != null)
                {
                    OnLoopClosed(polygon);
                }
                
                var objectOfInterests = GetObjectOfInterestsWithin(polygon);
                if (!objectOfInterests.IsNullOrEmpty())
                {
                    foreach (var interest in objectOfInterests)
                    {
                        interest.ActivateLazo();
                    }
                }
            }
            // Debug
            DebugCreateCubeAt(position);
        }

        private LazoPosition[] GetPolygon(int closedOffPosition)
        {
            var polygon = new List<LazoPosition>();
            var length = _listOfPositions.Count - closedOffPosition;

            var p1 = _listOfPositions[closedOffPosition].Position;
            var p2 = _listOfPositions[closedOffPosition + 1].Position;
            var p3 = _listOfPositions[_listOfPositions.Count - 1].Position;
            var p4 = _listOfPositions[_listOfPositions.Count - 2].Position;

            var centerPoint = GeometryUtilities.CenterPoint(new[] {p1, p2, p3, p4});
            var centerLazoPoint = new LazoPosition(_lazoProperties.TimeToLivePerPoint, centerPoint);
            
            polygon.Add(centerLazoPoint);
            var polygonRange = _listOfPositions.GetRange(closedOffPosition + 1, length - 2);
            polygon.AddRange(polygonRange);
            return polygon.ToArray();
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

        private ILazoWrapped[] GetObjectOfInterestsWithin(LazoPosition[] polygon)
        {
            List<ILazoWrapped> listOfObjects = new List<ILazoWrapped>();
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

        private float DistanceBetweenV3ToV2(Vector3 positionA, Vector3 positionB)
        {
            var posA = new Vector2(positionA.x, positionA.z);
            var posB = new Vector2(positionB.x, positionB.z);
            return Vector2.Distance(posA, posB);
        }
        #endregion

        #region Debug
        /*
         * DEBUG
         */
        private List<GameObject> _listOfDebugCubes = new List<GameObject>();
        
        public bool IsDebugging { get; set; }
        private void DebugCreateCubeAt(Vector3 position)
        {
            if (!IsDebugging) return;
            Debug.Log("Number of Cube: "+ _listOfDebugCubes.Count);
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = position;
            cube.GetComponent<BoxCollider>().enabled = false;
            _listOfDebugCubes.Add(cube);
        }

        private void DebugDestroyLastCubeOnList()
        {
            if (!IsDebugging) return;
            if (_listOfDebugCubes.Count > 0)
            {
                var cube = _listOfDebugCubes[0];
                _listOfDebugCubes.RemoveAt(0);
                GameObject.Destroy(cube);
            }
        }

        private void DebugClearListOfCubes()
        {
            if (!IsDebugging) return;
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