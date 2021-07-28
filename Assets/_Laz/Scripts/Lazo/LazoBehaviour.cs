using System.Linq;
using DG.Tweening;
using Shapes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Laz
{
    public class LazoBehaviour : MonoBehaviour
    {
        private const string SHADER_PROPERTY_ALPHA = "_Alpha";
        private const float TRAIL_FADE_DURATION = 0.25f;
        
        [SerializeField] private LineRenderer _lazoLineRenderer = null;
        [SerializeField] private ParticleSystem _lazoSparkleParticleSystem = null;
        [SerializeField] private Polygon _polygonShape = null;
        
        private Lazo _lazo;
        private float _elapsedCoolDown = 0;
        private ILazoColorProperties _lazoColors = null;

        public bool IsLazoing => _lazo.IsLazoing;
        public Lazo LazoModel => _lazo;
        
        public void Initialize(Lazo lazo, ILazoColorProperties lazoColors)
        {            
            _lazo = lazo;
            _lazoColors = lazoColors;
            _lazo.OnLazoLimitReached += HandleOnLazoLimitReached;
            _lazo.OnLoopClosed += HandleOnLoopClosed;
            _lazo.OnListOfLazoPositionsChanged += HandleOnListOfLazoPositionsChanged;
            _lazo.OnTimeToLiveStateChanged += HandleTimeToLiveStateChange;
            ClearLazoTrail();
        }

        public void ResetLazoLimit()
        {
            _lazo.ResetTravelledDistance();
        }

        public void CleanUp()
        {
            ClearLazoTrail();
            _lazo.CleanUp();
            _elapsedCoolDown = 0;
        }

        public void Reset()
        {
            SetupLazoColors();
            SetWholeLazoLoopAlpha(1);
            _lazo.Reset();
        }

        private void SetupLazoColors()
        {
            _lazoLineRenderer.colorGradient = _lazoColors.NormalGradient;
        }

        private void TurnLazoing(bool active)
        {
            if (active)
            {
                SetupLazoColors();
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
            
            var tween = _lazoLineRenderer.material.DOFloat(0, SHADER_PROPERTY_ALPHA, TRAIL_FADE_DURATION);
            tween.OnComplete(() =>
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

        private void HandleTimeToLiveStateChange(bool isFrozen)
        {
            _lazoLineRenderer.colorGradient = isFrozen ? _lazoColors.FrozenColor : _lazoColors.NormalGradient;
        }

        private void SetWholeLazoLoopAlpha(float alpha)
        {
            _lazoLineRenderer.material.SetFloat(SHADER_PROPERTY_ALPHA, alpha);
        }
        #endregion

    }
}
