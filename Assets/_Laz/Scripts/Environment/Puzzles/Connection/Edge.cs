using System;

namespace Laz
{
    public class Edge
    {
        private bool _isActivated = false;
        private Node _frontNode = null;
        private Node _backNode = null;

        public event Action OnEdgeCompleted;

        public Edge(Node frontNode, Node backNode, Action handleOnEdgeCompleted)
        {
            _frontNode = frontNode;
            _frontNode.OnIsActiveChanged += HandleOnNodeIsActiveChanged;
            _backNode = backNode;
            _backNode.OnIsActiveChanged += HandleOnNodeIsActiveChanged;
            OnEdgeCompleted += handleOnEdgeCompleted;
        }

        public void CompleteBackNodeConnection()
        {
            _backNode.CompletedConnection();
        }

        public void CleanUp()
        {
            _frontNode.OnIsActiveChanged -= HandleOnNodeIsActiveChanged;
            _backNode.OnIsActiveChanged -= HandleOnNodeIsActiveChanged;
            OnEdgeCompleted -= null;
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
                if (OnEdgeCompleted != null)
                {
                    OnEdgeCompleted();
                }

                CleanUp();
                return;
            }

            _backNode.CanActivate = _frontNode.IsActive;
        }
    }
}