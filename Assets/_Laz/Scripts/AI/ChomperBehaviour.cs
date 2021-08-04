using Pathfinding;
using UnityEngine;

namespace Laz
{
    public enum ChomperState
    {
        Idle,
        Detection,
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
        private Lazo _lazo = null;

        public void Initialize(Lazo lazo,
            IChomperProperties chomperProperties = null)
        {
            _lazo = lazo;
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
            _ai.maxSpeed = _chomperProperties.IdleSpeed;
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
            _ai.maxSpeed = _chomperProperties.IdleSpeed;
        }

        public void RayCastDidCollideWith(GameObject collidedGameObject)
        {
            var lazoWallBehaviour = collidedGameObject.GetComponent<LazoWallBehaviour>();
            if (lazoWallBehaviour != null)
            {
                OnDetected(lazoWallBehaviour.LazoWallPosition);
            }
        }

        private void OnDetected(LazoPosition lazoPosition)
        {
            if (_state != ChomperState.Detection)
            {
                Debug.Log("Idle -> Detected");
                _state = ChomperState.Detection;
                _aiChomperAgro.SetLazoPosition(lazoPosition);
                _ai.destination = lazoPosition.Position;
                _lazo.IsTimeToLiveFrozen = true;
            }
        }


        #region Mono
        private void Update()
        {
            DebugUIBehaviour.Instance.SetDebugText("State :"+_state);
            switch (_state)
            {
                case ChomperState.Idle:
                    _detectionBehaviour.OnDetectUpdate();
                    break;
                case ChomperState.Return:
                    //_detectionBehaviour.OnDetectUpdate();
                    break;
            }
        }

        private void FixedUpdate()
        {
            switch (_state)
            {
                case ChomperState.Idle:
                    _patrolBehaviour.PatrolCircularArea();
                    break;
                case ChomperState.Detection:
                    OnDetectionUpdate();
                    break;
                case ChomperState.Agro:
                    _aiChomperAgro.OnAgroUpdate();
                    break;
                case ChomperState.Return:
                    OnReturnUpdate();
                    break;
                default:
                    _patrolBehaviour.PatrolCircularArea();
                    break;
            }
        }

        private void HandleOnAgroEnded()
        {
            Debug.Log("Agro -> Return");
            _state = ChomperState.Return;
            _ai.destination = _originalPosition;
        }

        private void OnReturnUpdate()
        {
            if (_ai.reachedDestination)
            {
                Debug.Log("Return -> Idle");
                _state = ChomperState.Idle;
                _ai.maxSpeed = _chomperProperties.IdleSpeed;
            }
        }

        private void OnDetectionUpdate()
        {
            if (_ai.remainingDistance < 0.05f)
            {
                OnAgroStart();
            } 
            else if (_ai.isStopped)
            {
                _state = ChomperState.Return;
                _ai.destination = _originalPosition;
            }
        }
        
        private void OnAgroStart()
        {
            if (_state == ChomperState.Detection)
            {
                Debug.Log("Detected -> Agro");
                _state = ChomperState.Agro;
                _aiChomperAgro.StartAgroAt();
                _ai.maxSpeed = _chomperProperties.AgroSpeed;
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
