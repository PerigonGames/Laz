using System;
using UnityEngine;

namespace Laz
{
    public interface ILazoWrapped: ILifeCycle
    {
        void ActivateLazo();
        Vector3 Position { get; }
        event Action OnActivated;
        bool IsActivated { get; }
    }
}
