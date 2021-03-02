using UnityEngine;

namespace Laz
{
    public class LazoBehaviour : MonoBehaviour
    {
        private Lazo _lazo;

        private void OnEnable()
        {
            _lazo = new Lazo();
        }

        private void OnDisable()
        {
            _lazo = null;
        }
    }
}
