using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laz
{
    [RequireComponent(typeof(Collider))]
    public class Obstacle : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag(Tags.LazPlayer))
            {
                
            }
        }
    }
}
