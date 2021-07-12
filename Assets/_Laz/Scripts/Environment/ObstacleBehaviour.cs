using UnityEngine;

namespace Laz
{
    [RequireComponent(typeof(Collider))]
    public class ObstacleBehaviour : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag(Tags.LazPlayer))
            {
                LazCoordinatorBehaviour lazCoordinator = other.gameObject.GetComponent<LazCoordinatorBehaviour>();

                if (lazCoordinator != null)
                {
                    lazCoordinator.KillLaz();
                }
            }
        }
    }
}
