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
        private AIChomperAgroBehaviour _aiChomperAgroBehaviour = null;
        private ChomperState _state = ChomperState.Idle;

        public void Initialize(Lazo lazo,
            IChomperProperties chomperProperties = null)
        {            
            base.Initialize();
            _chomperProperties = chomperProperties ?? _chomperPropertiesScriptableObject;
            _patrolBehaviour.Initialize(_ai, _chomperProperties.IdleRadius);
            _detectionBehaviour.Initialize(_chomperProperties.AgroDetectionRadius, this);
            _aiChomperAgroBehaviour.Initialize(_ai, lazo);
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
            _state = ChomperState.Idle;
        }
        
        public void RayCastDidCollideWith(GameObject collidedGameObject)
        {
            // Check if it's a LazoWall
            // Need list of LazoPositions
            var lazoWallBehaviour = collidedGameObject.GetComponent<LazoWallBehaviour>();
            if (lazoWallBehaviour != null)
            {
                _aiChomperAgroBehaviour.StartAgroAt(lazoWallBehaviour.LazoWallPosition);
                _state = ChomperState.Agro;
                Debug.Log("Set to Agro");
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
            
            if (!TryGetComponent(out _aiChomperAgroBehaviour))
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
                    _detectionBehaviour.Detect();
                    break;
                case ChomperState.Agro:
                    _aiChomperAgroBehaviour.Agro();
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