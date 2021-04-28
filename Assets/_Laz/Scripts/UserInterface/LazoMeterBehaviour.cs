using UnityEngine;
using UnityEngine.UI;

namespace Laz
{
    public class LazoMeterBehaviour : MonoBehaviour
    {
        private const float SideMargin = 20f;
        
        [SerializeField] 
        private Image[] _fillLazoImages = null;
        [SerializeField] 
        private RectTransform _blob = null;
        private Lazo _lazo = null;
        
        public void Initialize(Lazo lazo)
        {
            _lazo = lazo;
            _lazo.OnLazoLimitChanged += HandleLimitChange;
            HandleLimitChange(1);
        }

        private void OnDestroy()
        {
            _lazo.OnLazoLimitChanged -= HandleLimitChange;
        }

        private void HandleLimitChange(float percentage)
        {
            foreach (var meter in _fillLazoImages)
            {
                meter.fillAmount = percentage;
            }
            
            _blob.anchorMin = new Vector2(percentage, 0);
            _blob.offsetMax = new Vector2(-SideMargin, _blob.offsetMax.y);
            _blob.offsetMin = new Vector2(-SideMargin, _blob.offsetMin.y);
        }
    }
}

