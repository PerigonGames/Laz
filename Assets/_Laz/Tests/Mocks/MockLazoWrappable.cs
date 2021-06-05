using System;
using Laz;
using UnityEngine;

namespace Tests
{
    public class MockLazoWrappable : ILazoWrapped
    {
        public void Reset()
        {
            
        }

        public void CleanUp()
        {
        }

        public void ActivateLazo()
        {
        }

        public Vector3 Position { get; }
        public event Action OnActivated;
        public bool IsActivated { get; }
    } 
}

