using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laz
{
    public class LazModelBehavior : MonoBehaviour
    {
        private LazMovementController _movementController;

        private void Awake()
        {
            _movementController = GetComponentInParent<LazMovementController>();
        }

        private void FixedUpdate()
        {
            if (_movementController.GetCurrentDirection.magnitude != 0)
                transform.rotation = Quaternion.LookRotation(_movementController.GetCurrentDirection, Vector3.up);
        }
    }
}
