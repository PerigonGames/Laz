using System;
using UnityEngine;

namespace Laz
{
    public class NodeConnectionBehaviour : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var lazo = other.GetComponent<LazoBehaviour>();
            if (lazo != null)
            {
                if (lazo.IsLazoing)
                {
                    
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            throw new NotImplementedException();
        }
    }
}