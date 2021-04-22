using UnityEngine;

namespace Laz
{
    public class LazModelBehavior : MonoBehaviour
    {
        private const string GlidingAnimationKey = "is_gliding";
        [SerializeField] private Animator _animator = null;
        private LazMovement _movement;

        public void Initialize(LazMovement movement)
        {
            _movement = movement;
            _movement.OnSpeedChanges += HandleOnSpeedChanges;
        }

        private void OnDestroy()
        {
            _movement.OnSpeedChanges -= HandleOnSpeedChanges;
        }

        private void FixedUpdate()
        {
            if (_movement.GetCurrentDirection.magnitude != 0)
            {
                transform.rotation = Quaternion.LookRotation(_movement.GetCurrentDirection, Vector3.up);
            }
        }

        private void HandleOnSpeedChanges(float speed)
        {
            var isGliding = speed > 0f;
            _animator.SetBool(GlidingAnimationKey, isGliding);
        }
    }
}
