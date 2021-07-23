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
            base.Initialize();
            _chomperProperties = chomperProperties ?? _chomperPropertiesScriptableObject;
            _patrolBehaviour.Initialize(_ai, _chomperProperties.IdleRadius);
            _detectionBehaviour.Initialize(_chomperProperties.AgroDetectionRadius, this);
            _aiChomperAgro = new AIChomperAgro(_ai, lazo, _chomperProperties.ExtraDistanceToTravel);
            _ai.maxSpeed = _chomperProperties.Speed;
        }

        public override void CleanUp()
        {
            base.CleanUp();
            _patrolBehaviour.CleanUp();
            _state = ChomperState.Idle;
        }

        public override void Reset()
        {
            base.Reset();
            _patrolBehaviour.Reset();
            _aiChomperAgro.Reset();
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

        protected override void Awake()
        {
            base.Awake();
            if (!TryGetComponent(out _ai))
            {
                PanicHelper.Panic(new Exception("ChomperBehaviour is missing AI Components - AIPath"));
            }

            if (!TryGetComponent(out _patrolBehaviour))
            {
                PanicHelper.Panic(new Exception("Chomper is missing a AIPatrolBehaviour"));
            }

            if (!TryGetComponent(out _detectionBehaviour))
            {
                PanicHelper.Panic(new Exception("Chomper is missing an AIDetectionBehaviour script"));
            }
        }

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
                default:
                    _patrolBehaviour.PatrolCircularArea();
                    break;
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