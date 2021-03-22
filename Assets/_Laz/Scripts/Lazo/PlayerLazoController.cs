using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Laz
{
    public class PlayerLazoController : MonoBehaviour
    {
        private LazMovementController _movementController;

        private void Awake()
        {
            _movementController = GetComponent<LazMovementController>();
        }

        /// <summary>
        /// Fire from player input in the Inspector
        /// </summary>
        /// <param name="context"></param>
        public void LazoTrigger(InputAction.CallbackContext context)
        {
            // If Lazo is Activated
            if (context.canceled == false)
            {
                _movementController.LazoBoostActivated();
            } // If Lazo is DeActivated
            else
            {
                _movementController.LazoBoostDeactivated();
            }
        }
    }
}
