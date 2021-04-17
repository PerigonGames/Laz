using UnityEngine;
using UnityEngine.InputSystem;

namespace Laz
{
    public class LazMovementBehaviour : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private LazMovement _movement = null;
        
        public void Initialize(LazMovement movement)
        {
            _movement = movement;
        }

        public void CleanUp()
        {
            _movement.CleanUp();
        }

        public void Reset()
        {
            _movement.Reset();
        }
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            _movement.SetSpeedComponent();
            _movement.DirectionComponent(_rigidbody.velocity.normalized);
            UpdateRigidBody();
        }

        private void UpdateRigidBody()
        {
            _rigidbody.velocity = _movement.GetVelocity;
        }

        /// <summary>
        /// Fired from Player input. Fired from the Inspector
        /// </summary>
        /// <param name="context"> Data with input information </param>
        public void OnMovementPressed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _movement.OnMovementPressed(context.ReadValue<Vector2>());
            }
            
            if (context.canceled)
            {
                _movement.OnMovementCancelled();
            }
        }
    }
}
