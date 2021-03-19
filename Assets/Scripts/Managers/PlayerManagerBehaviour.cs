using UnityEngine;

namespace Laz
{
    public class PlayerManagerBehaviour : MonoBehaviour
    {
        private LazoBehaviour _lazo = null;

        private void Awake()
        {
            _lazo = GetComponent<LazoBehaviour>();
        }

        public void Initialize()
        {

        }
    }
}

