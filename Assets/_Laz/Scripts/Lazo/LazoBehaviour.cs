using System.Linq;
using Shapes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Laz
{
    public class LazoBehaviour : MonoBehaviour
    {
        private Lazo _lazo;
        [SerializeField] private TrailRenderer _trail = null;
        [SerializeField] private bool TurnOnDebug = false;
        [SerializeField] private Polygon _polygonShape = null;

        private IBoost _boost = null;

        private float _elapsedCoolDown = 0;
        public bool IsLazoing => _lazo.IsLazoing;
        
        public void Initialize(Lazo lazo, IBoost boost)
        {            
            _boost = boost;
            
            _lazo = lazo;
            _lazo.IsDebugging = TurnOnDebug;
            _lazo.OnLazoLimitReached += HandleOnLazoLimitReached;
            _lazo.OnLoopClosed += HandleOnLoopClosed;
            
            _trail.time = _lazo.TimeToLivePerPoint;

        }

        public void ResetLazoLimit()
        {
            _lazo.ResetTravelledDistance();
        }

        public void CleanUp()
        {
            _lazo.CleanUp();
            _elapsedCoolDown = 0;
        }

        public void Reset()
        {
            _lazo.Reset();
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

        private void TurnLazoing(bool isOn)
        {
            _lazo.IsLazoing = isOn;
        }
        
        private void OnDestroy()
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

        private void HandleOnLoopClosed(LazoPosition[] positions)
        {
            if (TurnOnDebug && _polygonShape != null)
            {
                _polygonShape.points.Clear();
                _polygonShape.meshOutOfDate = true;
                _trail.Clear();
                var allPositions = positions.Select(lazoPosition => new Vector2(lazoPosition.Position.x, lazoPosition.Position.z)).ToList();
                foreach (var position in allPositions)
                {
                    _polygonShape.AddPoint(position);
                }
            }
        }

        private void HandleOnLazoLimitReached()
        {
            _trail.Clear();
        }
        #endregion

    }
}
