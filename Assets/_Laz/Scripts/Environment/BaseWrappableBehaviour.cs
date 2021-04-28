using UnityEngine;

namespace Laz
{
    public abstract class BaseWrappableBehaviour : MonoBehaviour
    {
        public abstract bool IsActivated { get; }
    }
}