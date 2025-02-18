using System;
using UnityEngine;

namespace Laz
{
    public class Planetoid : ILazoWrapped
    {
        private readonly IPlanetoidBehaviour _planetoidBehaviour = null;

        private bool _isActivated = false;

        public event Action OnPlanetoidActivated;

        public Vector3 Position => _planetoidBehaviour.Position;
        
        public bool IsActivated => _isActivated;

        public Planetoid(IPlanetoidBehaviour planetoidBehaviour)
        {
            _planetoidBehaviour = planetoidBehaviour;
        }

        public void ActivateLazo()
        {
            _isActivated = true;
            _planetoidBehaviour.LazoActivated();
            OnPlanetoidActivated?.Invoke();
        }

        public void CleanUp()
        {
            _isActivated = false;
            _planetoidBehaviour.CleanUp();
        }

        public void Reset()
        {
            _planetoidBehaviour.Reset();
        }
    }
}