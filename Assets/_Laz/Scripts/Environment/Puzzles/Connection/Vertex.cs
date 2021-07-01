using System;
using UnityEngine;

namespace Laz
{
    public class Vertex
    {
        private bool _isActivated = false;
        private Node _frontNode = null;
        private Node _backNode = null;

        public event Action OnVertexCompleted;

        public bool IsActivated => _isActivated;

        public Vertex(Node frontNode, Node backNode)
        {
            _frontNode = frontNode;
            _frontNode.OnIsActiveChanged += HandleOnNodeIsActiveChanged;
            _backNode = backNode;
            _backNode.OnIsActiveChanged += HandleOnNodeIsActiveChanged;
        }

        public void SetFrontNodeCanActivate()
        {
            _frontNode.CanActivate = true;
        }

        private void HandleOnNodeIsActiveChanged(bool isActive)
        {
            if (_isActivated)
            {
                return;
            }
            
            if (_frontNode.IsActive && _backNode.IsActive)
            {
                _isActivated = true;
                _frontNode.CompletedConnection();
                if (OnVertexCompleted != null)
                {
                    OnVertexCompleted();
                }

                return;
            }

            _backNode.CanActivate = _frontNode.IsActive;
        }
    }
}