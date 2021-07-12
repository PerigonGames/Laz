using PerigonGames;
using UnityEngine;

namespace Laz
{
    public class BoosterRingPuzzleActivateBehaviour : BaseCompletedPuzzleActivationBehaviour
    {
        public override void Initialize()
        {
            Reset();
        }

        public override void OnActivated()
        {
            transform.ResetScale();
        }

        public override void Reset()
        {
            transform.localScale = Vector3.zero;
        }
    }
}

