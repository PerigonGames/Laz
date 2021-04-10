using UnityEngine;
using UnityEngine.InputSystem;

namespace Laz
{
    public class LazoBehaviour : MonoBehaviour
    {
        private Lazo _lazo;
        [SerializeField] private TrailRenderer _trail = null;
        [SerializeField] private bool TurnOnDebug = false;

        private IBoost _boost = null;

        private float _elapsedCoolDown = 0;
        
        public void Initialize(Lazo lazo, IBoost boost)
        {
            _lazo = lazo;
            _lazo.IsDebugging = TurnOnDebug;
            _boost = boost;
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
        public void OnLazoPressed(InputAction.CallbackContext context)
        {
            if (context.performed && CanActivateLaz())
            {
                _boost.IsBoostActivated = true;
                TurnLazoing(true);
            } 
        }

        private void OnEnable()
        {
            _trail.time = _lazo.TimeToLivePerPoint;
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
            if (_lazo.IsLazoing)
            {
                _elapsedCoolDown = _lazo.CoolDown;
                _lazo.DidLazoLimitReached(transform.position);
                _lazo.RunLazoIfAble(transform.position, Time.deltaTime);
            }
            else
            {
                _elapsedCoolDown -= Time.deltaTime;
                _trail.Clear();
            }
        }

        private bool CanActivateLaz()
        {
            return _elapsedCoolDown <= 0;
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
