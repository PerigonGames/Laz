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
    
    public class ChomperBehaviour : EnemyBehaviour
    {
        [Header("Scriptable Object")]
        [SerializeField]
        private ChomperPropertiesScriptableObject _chomperPropertiesScriptableObject = null;
        
        // Dependencies
        private IChomperProperties _chomperProperties = null;
        private IAstarAI _ai = null;
        private AIPatrolBehaviour _patrolBehaviour = null;

        private ChomperState _state = ChomperState.Idle;

        public void Initialize(
            IChomperProperties chomperProperties = null)
        {            
            base.Initialize();
            _chomperProperties = chomperProperties ?? _chomperPropertiesScriptableObject;
            _patrolBehaviour.Initialize(_ai, _chomperProperties.IdleRadius);
        }

        public override void CleanUp()
        {
            base.CleanUp();
            _patrolBehaviour.CleanUp();
        }

        public override void Reset()
        {
            base.Reset();
            _patrolBehaviour.Reset();
        }
        
        #region Mono

        protected override void Awake()
        {
            base.Awake();
            if (!TryGetComponent(out _ai))
            {
                Debug.LogError("ChomperBehaviour is missing AI Components - AIPath");
            }

            if (!TryGetComponent(out _patrolBehaviour))
            {
                Debug.LogError("Chomper is missing a AIPatrolBehaviour");
            }
        }

        private void Update()
        {
            switch (_state)
            {
                case ChomperState.Idle:
                    _patrolBehaviour.PatrolCircularArea();
                    break;
                default:
                    _patrolBehaviour.PatrolCircularArea();
                    break;
            }
        }

        
        #endregion
        
        #region Gizmo
#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 1, 0, 0.25f);
            var patrolArea = _originalPosition == Vector3.zero ? transform.position : _originalPosition;
            Gizmos.DrawSphere(patrolArea, _chomperPropertiesScriptableObject.IdleRadius);
        }
#endif
        #endregion
    }

   
}