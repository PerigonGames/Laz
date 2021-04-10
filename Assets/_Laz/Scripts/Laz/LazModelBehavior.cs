using UnityEngine;

namespace Laz
{
    public class LazModelBehavior : MonoBehaviour
    {
        private LazMovement _movement;

        public void Initialize(LazMovement movement)
        {
            _movement = movement;
        }

        private void FixedUpdate()
        {
            if (_movement.GetCurrentDirection.magnitude != 0)
                transform.rotation = Quaternion.LookRotation(_movement.GetCurrentDirection, Vector3.up);
        }
    }
}
