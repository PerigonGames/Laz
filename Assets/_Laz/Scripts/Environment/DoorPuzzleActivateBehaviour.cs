using UnityEngine;

namespace Laz
{
    public class DoorPuzzleActivateBehaviour : BasePuzzleActivateBehaviour
    {
        private Vector3 InitializeSize;
        
        public override void Initialize()
        {
            InitializeSize = transform.localScale;
            Reset();
        }

        public override void Reset()
        {
            transform.localScale = InitializeSize;
        }

        public override void OnActivated()
        {
            transform.localScale = Vector3.zero;
        }
    }
}
