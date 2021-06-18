using System;
using TMPro;
using UnityEngine;

namespace Laz
{
    public class DebugUIBehaviour : MonoBehaviour
    {
        [SerializeField] private TMP_Text _lazoVelocity = null;
        [SerializeField] private TMP_Text _genericDebugTextOnWall = null;
        
        private LazMovement _movement = null;
        private static DebugUIBehaviour _instance = null;
        
        public static DebugUIBehaviour Instance => _instance;
        
        public void Initialize(LazMovement movement)
        {
            _movement = movement;
            _movement.OnSpeedChanges += HandleVelocityChange;
        }

        public void SetDebugText(string text)
        {
            _genericDebugTextOnWall.text = text;
        }

        private void Awake()
        {
            _instance = this;
        }

        private void OnDestroy()
        {
            _movement.OnSpeedChanges -= HandleVelocityChange;
        }

        private void HandleVelocityChange(float velocity)
        {
            _lazoVelocity.text = $"Velocity: {velocity}";
        }

    }
}
