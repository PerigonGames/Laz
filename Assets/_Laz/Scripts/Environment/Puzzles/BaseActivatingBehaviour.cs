using UnityEngine;

namespace Laz
{
    public abstract class BaseActivatingBehaviour : MonoBehaviour
    {
        public abstract bool IsActivated { get; }
    }
}