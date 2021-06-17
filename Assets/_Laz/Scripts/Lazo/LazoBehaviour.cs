using System.Linq;
using Shapes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Laz
{
    public class LazoBehaviour : MonoBehaviour
    {
        private const string ShaderPropertyAlpha = "_Alpha";
        
        [SerializeField] private LineRenderer _lazoLineRenderer = null;
        [SerializeField] private bool TurnOnDebug = false;
        [SerializeField] private Polygon _polygonShape = null;
        
        private Lazo _lazo;
        private float _elapsedCoolDown = 0;

        public bool IsLazoing => _lazo.IsLazoing;
        
        public void Initialize(Lazo lazo)
        {            
            _lazo = lazo;
            _lazo.IsDebugging = TurnOnDebug;
            _lazo.OnLazoLimitReached += HandleOnLazoLimitReached;
            _lazo.OnLoopClosed += HandleOnLoopClosed;
            _lazo.OnLazoPositionAdded += HandleOnLazoPositionAdded;
            ClearLineRenderer();
            //_trail.time = _lazo.TimeToLivePerPoint;
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

        private void TurnLazoing(bool active)
        {
            _lazo.SetLazoActive(active);
        }
        
        private bool CanActivateLaz()
        {
            return _elapsedCoolDown <= 0;
        }

        private void ClearLineRenderer()
        {
            _lazoLineRenderer.SetPositions(new Vector3[] { });
        }
        
        #region Mono
        /// <summary>
        /// USED IN INSPECTOR
        /// Fire from player input in the Inspector
        /// </summary>
        /// <param name="context"></param>
        public void OnLazoPressed(InputAction.CallbackContext context)
        {
            if (context.started && CanActivateLaz())
            {
                TurnLazoing(true);
            }
            else if (context.canceled && !CanActivateLaz())
            {
                TurnLazoing(false);
            }
        }

        private void Awake()
        {
            SetWholeLazoLoopAlpha(1f);
        }

        private void OnDestroy()
        {
            _lazo.OnLazoLimitReached -= HandleOnLazoLimitReached;
            _lazo.OnLoopClosed -= HandleOnLoopClosed;
            _lazo = null;
        }

        private void Update()
        {
            //_trail.emitting = _lazo.IsLazoing;
            if (_lazo.IsLazoing)
            {
                _elapsedCoolDown = _lazo.CoolDown;
                _lazo.HandleIfLazoLimitReached(transform.position);
                _lazo.RunLazoIfAble(transform.position, Time.deltaTime);
            }
            else
            {
                _elapsedCoolDown -= Time.deltaTime;
                ClearLineRenderer();
            }
        }
        #endregion
        
        #region Delegate

        private void HandleOnLoopClosed(LazoPosition[] positions)
        {
            if (TurnOnDebug && _polygonShape != null)
            {
                _polygonShape.points.Clear();
                _polygonShape.meshOutOfDate = true;
                ClearLineRenderer();
                var allPositions = positions.Select(lazoPosition => new Vector2(lazoPosition.Position.x, lazoPosition.Position.z)).ToList();
                foreach (var position in allPositions)
                {
                    _polygonShape.AddPoint(position);
                }
            }
        }

        private void HandleOnLazoLimitReached()
        {
            ///ClearLineRenderer();
            
        }

        private void HandleOnLazoPositionAdded(Vector3[] positions)
        {
            _lazoLineRenderer.positionCount = positions.Length;
            _lazoLineRenderer.SetPositions(positions);
        }

        private void SetWholeLazoLoopAlpha(float alpha)
        {
            _lazoLineRenderer.material.SetFloat(ShaderPropertyAlpha, alpha);
        }
        #endregion

    }
}
