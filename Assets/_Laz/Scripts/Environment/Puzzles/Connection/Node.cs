using System;

namespace Laz
{
    public class Node
    {
        private bool _canActivate = false;
        private bool _isActive = false;
        
        public event Action<bool> OnCanActivateChanged;
        public event Action<Node> OnIsActiveChanged;

        public event Action OnNodeCompleted;

        public bool CanActivate
        {
            get => _canActivate;

            set
            {
                _canActivate = value;
                OnCanActivateChanged?.Invoke(value);
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                OnIsActiveChanged?.Invoke(this);
            }
        }

        public void CompletedConnection()
        {
            _canActivate = false;
            _isActive = false;
            OnNodeCompleted?.Invoke();
        }
    }
}