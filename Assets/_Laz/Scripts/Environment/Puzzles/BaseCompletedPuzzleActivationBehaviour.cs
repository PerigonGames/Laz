using UnityEngine;

namespace Laz
{    
    public abstract class BaseCompletedPuzzleActivationBehaviour : MonoBehaviour
    {
        public abstract void Initialize();
        
        public abstract void OnActivated();
        
        public virtual void Reset() { }

        public virtual void CleanUp() { }
    }
}
