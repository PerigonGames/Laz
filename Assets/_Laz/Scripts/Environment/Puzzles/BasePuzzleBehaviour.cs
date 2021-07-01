using System;
using UnityEngine;

namespace Laz
{
    public abstract class BasePuzzleBehaviour : MonoBehaviour
    {
        private bool _isActived = false;
        public virtual bool IsActivated => _isActived;
        public event Action OnPuzzleCompleted;

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