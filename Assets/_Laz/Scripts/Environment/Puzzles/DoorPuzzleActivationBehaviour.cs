using UnityEngine;

namespace Laz
{
    public class DoorPuzzleActivationBehaviour : BaseCompletedPuzzleActivationBehaviour
    {
        private Vector3 _initializeSize;
        
        public override void Initialize()
        {
            _initializeSize = transform.localScale;
            Reset();
        }

        public override void Reset()
        {
            transform.localScale = _initializeSize;
        }

        public override void OnActivated()
        {
            transform.localScale = Vector3.zero;
        }
    }
}
