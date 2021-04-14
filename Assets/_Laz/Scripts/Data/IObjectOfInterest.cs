using System;
using UnityEngine;

namespace Laz
{
    public interface IObjectOfInterest: ILifeCycle
    {
        void OnLazoActivated();
        Vector3 Position { get; }

        void Reset();

        event Action OnActivated;
    }
}
