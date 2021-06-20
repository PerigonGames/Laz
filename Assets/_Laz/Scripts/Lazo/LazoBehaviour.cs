using System.Linq;
using DG.Tweening;
using Shapes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Laz
{
    public class LazoBehaviour : MonoBehaviour
    {
        private const string ShaderPropertyAlpha = "_Alpha";
        private const float TrailFadeDuration = 0.25f;
        
        [SerializeField] private LineRenderer _lazoLineRenderer = null;
        [SerializeField] private ParticleSystem _lazoSparkleParticleSystem = null;
        [SerializeField] private Polygon _polygonShape = null;
        
        private Lazo _lazo;
        private float _elapsedCoolDown = 0;
        private Tweener materialTween;

        public bool IsLazoing => _lazo.IsLazoing;
        
        public void Initialize(Lazo lazo)
        {            
            _lazo = lazo;
            _lazo.OnLazoLimitReached += HandleOnLazoLimitReached;
            _lazo.OnLoopClosed += HandleOnLoopClosed;
            _lazo.OnListOfLazoPositionsChanged += HandleOnListOfLazoPositionsChanged;
            ClearLazoTrail();
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
            if (active)
            {
                SetWholeLazoLoopAlpha(1);
                _lazoSparkleParticleSystem.Play();
            }
            else
            {
                ClearLazoTrail();
            }
            
            _lazo.SetLazoActive(active);
        }
        
        private bool CanActivateLaz()
        {
            return _elapsedCoolDown <= 0;
        }

        private void ClearLazoTrail()
        {
            _lazoSparkleParticleSystem.Clear();
            
            materialTween = _lazoLineRenderer.material.DOFloat(0, ShaderPropertyAlpha, TrailFadeDuration);
            materialTween.OnComplete(() =>
            {
                SetWholeLazoLoopAlpha(1);
                _lazoLineRenderer.positionCount = 0;
            });
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
            _lazo.OnListOfLazoPositionsChanged -= HandleOnListOfLazoPositionsChanged;
            _lazo = null;
        }

        private void Update()
        {
            if (_lazo.IsLazoing)
            {
                _elapsedCoolDown = _lazo.CoolDown;
                _lazo.HandleIfLazoLimitReached(transform.position);
            }
            else
            {
                _lazoSparkleParticleSystem.Clear();
                _elapsedCoolDown -= Time.deltaTime;
            }
        }

        // Late Update is so Lazo can react to Limit Reached.
        private void LateUpdate()
        {
            if (_lazo.IsLazoing)
            {
                _lazo.RunLazoIfAble(transform.position, Time.deltaTime);
            }
        }

        #endregion
        
        #region Delegate

        private void HandleOnLoopClosed(LazoPosition[] positions)
        {
            if (_polygonShape != null)
            {
                _polygonShape.points.Clear();
                _polygonShape.meshOutOfDate = true;
                ClearLazoTrail();
                var allPositions = positions.Select(lazoPosition => new Vector2(lazoPosition.Position.x, lazoPosition.Position.z)).ToList();
                foreach (var position in allPositions)
                {
                    _polygonShape.AddPoint(position);
                }
            }
        }

        private void HandleOnLazoLimitReached()
        {
            ClearLazoTrail();
        }

        private void HandleOnListOfLazoPositionsChanged(Vector3[] positions)
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
