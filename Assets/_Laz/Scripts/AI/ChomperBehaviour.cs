using System;
using Pathfinding;
using UnityEngine;

namespace Laz
{
    public enum ChomperState
    {
        Idle,
        Agro,
        Return
    }

    public class ChomperBehaviour : EnemyBehaviour, IAIDetectionDataSource
    {
        [Header("Scriptable Object")]
        [SerializeField]
        private ChomperPropertiesScriptableObject _chomperPropertiesScriptableObject = null;

        // Dependencies
        private IChomperProperties _chomperProperties = null;
        private IAstarAI _ai = null;
        private AIPatrolBehaviour _patrolBehaviour = null;
        private AIDetectionBehaviour _detectionBehaviour = null;

        private AIChomperAgro _aiChomperAgro = null;
        private ChomperState _state = ChomperState.Idle;

        public void Initialize(Lazo lazo,
            IChomperProperties chomperProperties = null)
        {
            if (!TryGetComponent(out _ai))
            {
                Debug.LogError("ChomperBehaviour is missing AI Components - AIPath");
            }

            if (!TryGetComponent(out _patrolBehaviour))
            {
                Debug.LogError("Chomper is missing a AIPatrolBehaviour");
            }
            
            if (!TryGetComponent(out _detectionBehaviour))
            {
                Debug.LogError("Chomper is missing a AIDetectionBehaviour");
            }

            base.Initialize();
            _chomperProperties = chomperProperties ?? _chomperPropertiesScriptableObject;
            _patrolBehaviour.Initialize(_ai, _chomperProperties.IdleRadius);
            _detectionBehaviour.Initialize(_chomperProperties.AgroDetectionRadius, this);
            _aiChomperAgro = new AIChomperAgro(_ai, lazo, _chomperProperties.ExtraDistanceToTravel);
            _aiChomperAgro.OnChomperReachedEndOfLazo += HandleOnAgroEnded;
            _ai.maxSpeed = _chomperProperties.Speed;
        }

        public override void CleanUp()
        {
            base.CleanUp();
            _patrolBehaviour.CleanUp();
            _aiChomperAgro.CleanUp();
            _aiChomperAgro.OnChomperReachedEndOfLazo -= HandleOnAgroEnded;
            _state = ChomperState.Idle;
        }

        public override void Reset()
        {
            base.Reset();
            _patrolBehaviour.Reset();
            _aiChomperAgro.Reset();
            _aiChomperAgro.OnChomperReachedEndOfLazo += HandleOnAgroEnded;
            _state = ChomperState.Idle;
        }

        public void RayCastDidCollideWith(GameObject collidedGameObject)
        {
            var lazoWallBehaviour = collidedGameObject.GetComponent<LazoWallBehaviour>();
            if (lazoWallBehaviour != null)
            {
                _aiChomperAgro.StartAgroAt(lazoWallBehaviour.LazoWallPosition);
                _state = ChomperState.Agro;
            }
        }

        #region Mono

        private void Update()
        {
            switch (_state)
            {
                case ChomperState.Idle:
                    _patrolBehaviour.PatrolCircularArea();
                    _detectionBehaviour.OnDetectUpdate();
                    break;
                case ChomperState.Agro:
                    _aiChomperAgro.OnAgroUpdate();
                    break;
                case ChomperState.Return:
                    OnReturnUpdate();
                    _detectionBehaviour.OnDetectUpdate();
                    break;
                default:
                    _patrolBehaviour.PatrolCircularArea();
                    break;
            }
        }

        private void HandleOnAgroEnded()
        {
            _state = ChomperState.Return;
            _ai.destination = _originalPosition;
        }

        private void OnReturnUpdate()
        {
            if (_ai.reachedEndOfPath)
            {
                _state = ChomperState.Idle;
            }
        }

        #endregion

#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 1, 0, 0.25f);
            var patrolArea = _originalPosition == Vector3.zero ? transform.position : _originalPosition;
            Gizmos.DrawSphere(patrolArea, _chomperPropertiesScriptableObject.IdleRadius);
        }
#endif
    }
}
