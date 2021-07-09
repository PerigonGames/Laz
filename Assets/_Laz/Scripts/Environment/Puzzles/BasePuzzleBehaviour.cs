using System;
using UnityEngine;

namespace Laz
{
    public abstract class BasePuzzleBehaviour : MonoBehaviour
    {
        private bool _isActivated = false;
        public virtual bool IsActivated => _isActivated;
        public event Action OnPuzzleCompleted;
        
        public virtual void Initialize()
        {
            _isActivated = false;
        }

        public virtual void CleanUp()
        {
            _isActivated = false;
            OnPuzzleCompleted = null;
        }

        public virtual void Reset()
        {
            _isActivated = false;
        }
        
        protected void Activate()
        {
            _isActivated = true;
            if (OnPuzzleCompleted != null)
            {
                OnPuzzleCompleted();
            }
        }
    }
}