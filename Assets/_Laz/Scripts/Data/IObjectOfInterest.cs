using UnityEngine;

namespace Laz
{
    public interface IObjectOfInterest
    {
        void OnLazoActivated();
        Vector3 Position { get; }
    }
}
