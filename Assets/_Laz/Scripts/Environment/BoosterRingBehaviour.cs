using UnityEngine;

namespace Laz 
{
    public class BoosterRingBehaviour : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var boost = other.GetComponent<IBoost>();
            var laz = other.GetComponent<LazoBehaviour>();
            if (boost != null && laz != null)
            {
                if (laz.IsLazoing)
                {
                    laz.ResetLazoLimit();
                    boost.SetBoostActive(true);
                }
            }
        }
    }
}