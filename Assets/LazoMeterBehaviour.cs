using UnityEngine;
using UnityEngine.UI;

namespace Laz
{
    public class LazoMeterBehaviour : MonoBehaviour
    {
        [SerializeField] 
        private Image _lazoMeter = null;
        private Lazo _lazo = null;
        
        public void Initialize(Lazo lazo)
        {
            _lazo = lazo;
            _lazo.OnLazoLimitChanged += HandleLimitChange;
        }

        private void OnDestroy()
        {
            _lazo.OnLazoLimitChanged -= HandleLimitChange;
        }

        private void HandleLimitChange(float percentage)
        {
            _lazoMeter.transform.localScale = new Vector3(percentage, 1, 1);
        }
    }
}

