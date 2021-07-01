using System;
using UnityEngine;

namespace Laz
{
    public abstract class BasePuzzleBehaviour : MonoBehaviour
    {
        private bool _isActived = false;
        public virtual bool IsActivated => _isActived;
        public event Action OnPuzzleCompleted;
        
        public virtual void Initialize()
        {
            _isActived = false;
        }

        public virtual void CleanUp()
        {
            _isActived = false;
        }

        public virtual void Reset()
        {
            _isActived = false;
        }
        
        protected void Activate()
        {
            _isActived = true;
            if (OnPuzzleCompleted != null)
            {
                OnPuzzleCompleted();
            }
        }
    }
}