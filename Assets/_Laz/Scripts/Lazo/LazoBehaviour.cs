using UnityEngine;
using UnityEngine.InputSystem;

namespace Laz
{
    public class LazoBehaviour : MonoBehaviour
    {
        private Lazo _lazo;
        [SerializeField] private TrailRenderer _trail = null;
        [SerializeField] private bool TurnOnDebug = false;
        
        [SerializeField] private LazoPropertiesScriptableObject _properties = null;
        private LazMovementBehaviour _movementBehaviour = null;
        private IObjectOfInterest[] _objectsOfInterest = null;
        
        public void Initialize(LazMovementBehaviour _movement, IObjectOfInterest[] objectsOfInterest)
        {
            _objectsOfInterest = objectsOfInterest;
            _movementBehaviour = _movement;
        }
        
        /// <summary>
        /// Turns Lazoing on or off
        /// </summary>
        /// <param name="isOn">is On</param>
        private void TurnLazoing(bool isOn)
        {
            _lazo.IsLazoing = isOn;
        }
        
        /// <summary>
        /// USED IN INSPECTOR
        /// Fire from player input in the Inspector
        /// </summary>
        /// <param name="context"></param>
        public void LazoTrigger(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                TurnLazoing(true);
                _movementBehaviour.LazoBoostActivated();
            } 
            
            if (context.canceled)
            {
                TurnLazoing(false);
                _movementBehaviour.LazoBoostDeactivated();
            }
        }

        private void OnEnable()
        {
            _lazo = new Lazo(_properties, _objectsOfInterest, TurnOnDebug);
            _trail.time = _properties.TimeToLivePerPoint;
            _lazo.OnLazoLimitReached += HandleOnLazoLimitReached;
            _lazo.OnLoopClosed += HandleOnLoopClosed;
        }

        private void OnDisable()
        {
            _lazo.OnLazoLimitReached -= HandleOnLazoLimitReached;
            _lazo.OnLoopClosed -= HandleOnLoopClosed;
            _lazo = null;
        }

        private void Update()
        {
            _trail.emitting = _lazo.IsLazoing;
            if (!_lazo.IsLazoing)
            {
                _trail.Clear();
                return;
            }

            _lazo.DidLazoLimitReached(transform.position);
            _lazo.RunLazoIfAble(transform.position, Time.deltaTime);
        }
        
        #region Delegate

        private void HandleOnLoopClosed()
        {
            _trail.Clear();
        }

        private void HandleOnLazoLimitReached()
        {
            _trail.Clear();
        }
        #endregion

    }
}
