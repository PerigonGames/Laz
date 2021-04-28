using UnityEngine;

namespace Laz
{
    public class DoorPuzzleActivateBehaviour : BasePuzzleActivateBehaviour
    {
        private readonly Vector3 InitializeSize = new Vector3(14, 1, 1);
        
        public override void Initialize()
        {
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
