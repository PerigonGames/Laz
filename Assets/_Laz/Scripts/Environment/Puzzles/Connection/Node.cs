using System;

namespace Laz
{
    public class Node
    {
        private bool _canActivate = false;
        private bool _isActive = false;

        public event Action<bool> OnCanActivateChanged;
        public event Action<bool> OnIsActiveChanged;

        public event Action OnNodeCompleted;

        public bool CanActivate
        {
            get => _canActivate;

            set
            {
                _canActivate = value;
                if (OnCanActivateChanged != null)
                {
                    OnCanActivateChanged(value);
                }
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                if (OnIsActiveChanged != null)
                {
                    OnIsActiveChanged(value);
                }
            }
        }

        public void CompletedConnection()
        {
            _canActivate = false;
            _isActive = false;
            if (OnNodeCompleted != null)
            {
                OnNodeCompleted();
            }
        }
    }
}