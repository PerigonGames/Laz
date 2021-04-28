using UnityEngine;

namespace Laz
{
    public class BoosterRingPuzzleActivateBehaviour : BasePuzzleActivateBehaviour
    {
        public override void Initialize()
        {
            Reset();
        }

        public override void OnActivated()
        {
            transform.localScale = Vector3.one;
        }

        public override void Reset()
        {
            transform.localScale = Vector3.zero;
        }
    }
}

