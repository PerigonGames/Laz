using TMPro;
using UnityEngine;

namespace Laz
{
    public class DebugUIBehaviour : MonoBehaviour
    {
        [SerializeField] private TMP_Text _lazoVelocity = null;
        
        private LazMovement _movement = null;
        
        public void Initialize(LazMovement movement)
        {
            _movement = movement;
            _movement.OnSpeedChanges += HandleVelocityChange;
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
