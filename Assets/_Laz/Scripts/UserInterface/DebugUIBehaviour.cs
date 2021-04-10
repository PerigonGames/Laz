using System;
using TMPro;
using UnityEngine;

namespace Laz
{
    public class DebugUIBehaviour : MonoBehaviour
    {
        [SerializeField] private TMP_Text _lazoText = null;
        [SerializeField] private TMP_Text _lazoVelocity = null;
        
        private Lazo _lazo = null;
        private LazMovement _movement = null;
        
        public void Initialize(Lazo lazo, LazMovement movement)
        {
            _lazo = lazo;
            _movement = movement;
            _lazo.OnLazoLimitChanged += HandleLimitChange;
            _movement.OnSpeedChanges += HandleVelocityChange;
        }

        private void OnDestroy()
        {
            _lazo.OnLazoLimitChanged -= HandleLimitChange;
            _movement.OnSpeedChanges -= HandleVelocityChange;
        }

        private void HandleLimitChange(float percentage)
        {
            _lazoText.text = $"Distance Left: {percentage}";
        }

        private void HandleVelocityChange(float velocity)
        {
            _lazoVelocity.text = $"Velocity: {velocity}";
        }

    }
}
