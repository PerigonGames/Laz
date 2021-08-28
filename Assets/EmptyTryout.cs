using UnityEngine;

namespace Laz
{
    public class EmptyTryout : MonoBehaviour
    {
        [SerializeField] private PatrolBehaviour _behaviour;

        private void Awake()
        {
            if (_behaviour != null)
            {
                _behaviour.Initialize();
            }
        }
    }
}
