using UnityEngine;

namespace Laz
{    
    public abstract class BasePuzzleActivateBehaviour : MonoBehaviour, ILifeCycle
    {
        public abstract void Initialize();
        
        public abstract void OnActivated();
        
        public virtual void Reset() { }

        public virtual void CleanUp() { }
    }
}
