using System;
using UnityEngine;

namespace Laz
{
    public abstract class BasePuzzleBehaviour : MonoBehaviour
    {
        private bool _isPuzzleActivated = false;
        public bool IsPuzzleActivated => _isPuzzleActivated;
        public event Action OnPuzzleCompleted;
        
        public virtual void Initialize()
        {
            _isPuzzleActivated = false;
        }

        public virtual void CleanUp()
        {
            _isPuzzleActivated = false;
            OnPuzzleCompleted = null;
        }

        public virtual void Reset()
        {
            _isPuzzleActivated = false;
        }
        
        protected void ActivatePuzzle()
        {
            _isPuzzleActivated = true;
            if (OnPuzzleCompleted != null)
            {
                OnPuzzleCompleted();
            }
        }
    }
}