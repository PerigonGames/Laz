using System;
using PerigonGames;
using UnityEngine;

namespace Laz
{
    public class NodeConnectionBehaviour : MonoBehaviour
    {
        private Node _node = null;
        
        public void Initialize(Node node)
        {
            _node = node;
            _node.OnCanActivateChanged += HandleOnOnCanActivateChanged;
        }

        private void CleanUp()
        {
            _node.OnCanActivateChanged -= HandleOnOnCanActivateChanged;
            _node = null;
        }

        private void Reset()
        {
            
        }

        private void HandleOnOnCanActivateChanged(bool canActivate)
        {
            if (canActivate)
            {
                transform.localScale = Vector3.one * 2;
            }
            else
            {
                transform.ResetScale();
            }
        }

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