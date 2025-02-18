using System;
using System.Collections.Generic;
using System.Linq;
using PerigonGames;
using UnityEngine;

namespace Laz
{
    public partial class Lazo
    {
        private readonly ILazoProperties _lazoProperties = null;
        private readonly ILazoWrapped[] _wrappableObjects = null;
        private readonly IBoost _boost = null;
        private List<LazoPosition> _listOfPositions = new List<LazoPosition>();

        private bool _isLazoing = false;
        private float _rateOfRecordingTimerElapsed = 0;
        private float _travelledDistance = 0;
        private Vector3? _lastPosition = null;
        private bool _isTimeToLiveFrozen = false;

        public event Action<LazoPosition[]> OnLoopClosed;
        public event Action OnLazoLimitReached;
        public event Action<float> OnLazoLimitChanged;
        public event Action OnLazoDeactivated;
        public event Action<Vector3[]> OnListOfLazoPositionsChanged;
        public event Action<bool> OnTimeToLiveStateChanged;

        public float CoolDown => _lazoProperties.CoolDown;
        public List<LazoPosition> GetListOfLazoPositions => _listOfPositions;
        public bool IsTimeToLiveFrozen
        {
            get => _isTimeToLiveFrozen;
            set
            {
                _isTimeToLiveFrozen = value;
                OnTimeToLiveStateChanged?.Invoke(value);
            }
        }

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

        public bool IsLazoing => _isLazoing;

        public Lazo(ILazoProperties properties, ILazoWrapped[] wrappableObjects, IBoost boost)
        {
            _wrappableObjects = wrappableObjects ?? new ILazoWrapped[] { };
            _lazoProperties = properties;
            _boost = boost;
            Reset();
        }

        public void CleanUp()
        {
            IsTimeToLiveFrozen = false;
            _isLazoing = false;
            _rateOfRecordingTimerElapsed = 0;
            _travelledDistance = 0;
            _lastPosition = null;
            CleanUpWall();
        }

        public void Reset()
        {
            _isTimeToLiveFrozen = false;
            _isLazoing = false;
            _rateOfRecordingTimerElapsed = 0;
            CleanUpWall();
            ResetTravelledDistance();
            _lastPosition = null;
        }

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

        public void SetLazoActive(bool activate)
        {
            _isLazoing = activate;
            if (_isLazoing)
            {
                _boost.SetBoostActive(activate);
            }
            else
            {
                CreateFakeLazoLineIfNeeded();
                OnLazoDeactivated?.Invoke();
                Reset();
            }
        }

        public void HandleIfLazoLimitReached(Vector3 position)
        {
            if (_lastPosition == null)
            {
                _lastPosition = position;
                return;
            }

            TravelledDistance -= DistanceBetweenV3ToV2((Vector3) _lastPosition, position);
            _lastPosition = position;

            if (TravelledDistance <= 0)
            {
                OnLazoLimitReached?.Invoke();
                SetLazoActive(false);
                _lastPosition = null;
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

            AddToListOfLazoPositions(new LazoPosition(_lazoProperties.TimeToLivePerPoint, position));

            if (IsClosedLoop(out var closedOffPosition))
            {
                var closedLoopPolygon = GetClosedLoopPolygon(closedOffPosition);
                if (OnLoopClosed != null)
                {
                    OnLoopClosed(closedLoopPolygon);
                }

                if (!_isTimeToLiveFrozen)
                {
                    KillOffTailEndOfLazoFrom(closedOffPosition, closedLoopPolygon);
                } 

                var objectOfInterests = GetObjectOfInterestsWithin(closedLoopPolygon);
                if (!objectOfInterests.IsNullOrEmpty())
                {
                    foreach (var interest in objectOfInterests)
                    {
                        interest.ActivateLazo();
                    }

                    ResetTravelledDistance();
                    _boost.SetBoostActive(true);
                }
            }
        }

        private LazoPosition[] GetClosedLoopPolygon(int closedOffPosition)
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



        #region Helper


        private void RemoveOldestPointIfNeeded(float deltaTime)
        {
            _listOfPositions = _listOfPositions.Select(lazoPosition =>
            {
                var time = deltaTime * TimeToLiveMutator();
                lazoPosition.DecrementTimeToLiveBy(time);
                return lazoPosition;
            }).Where(lazoPosition => !lazoPosition.IsTimeBelowZero).ToList();
            OnLazoPositionsChanged();
        }

        private float TimeToLiveMutator()
        {
            return IsTimeToLiveFrozen ? 0 : 1;
        }

        private void AddToListOfLazoPositions(LazoPosition position)
        {
            _listOfPositions.Add(position);
            SpawnWall(position);
            OnLazoPositionsChanged();
        }

        private void OnLazoPositionsChanged()
        {
            if (OnListOfLazoPositionsChanged != null)
            {
                OnListOfLazoPositionsChanged(_listOfPositions.Select(x => x.Position).Reverse().ToArray());
            }
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

        private ILazoWrapped[] GetObjectOfInterestsWithin(LazoPosition[] lazoPolygon)
        {
            var listOfObjects = new List<ILazoWrapped>();
            var arrayOfLazoPositions = lazoPolygon.Select(position => position.Position).ToArray();
            var inactiveWrappableObjects = _wrappableObjects.Where(wrappable => !wrappable.IsActivated);
            foreach (var piece in inactiveWrappableObjects)
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

    }
}
