using TMPro;
using UnityEngine;

namespace Laz
{
    public class LazoUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _lazoText = null;

        public void Initialize(Lazo lazo)
        {
            lazo.OnLazoLimitChanged += HandleLimitChange;
        }

        private void HandleLimitChange(float percentage)
        {
            _lazoText.text = $"Distance Left: {percentage}";
        }
    }
}

