using UnityEngine;

namespace Laz
{
    public class LazModelBehavior : MonoBehaviour
    {
        private LazMovementBehaviour _movementBehaviour;

        public void Initialize(LazMovementBehaviour movementBehaviour)
        {
            _movementBehaviour = movementBehaviour;
        }

        private void FixedUpdate()
        {
            if (_movementBehaviour.GetCurrentDirection.magnitude != 0)
                transform.rotation = Quaternion.LookRotation(_movementBehaviour.GetCurrentDirection, Vector3.up);
        }
    }
}
