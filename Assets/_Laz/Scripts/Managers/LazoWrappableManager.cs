using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Laz
{
    public class LazoWrappableManager
    {
        private readonly List<Planetoid> _listOfPlanetoids = new List<Planetoid>();

        public ILazoWrapped[] WrappableObjects { get; } = null;

        public LazoWrappableManager(PlanetoidBehaviour[] planetoids)
        {
            SetupPlanetoids(planetoids);
            WrappableObjects = _listOfPlanetoids.ToArray();
        }

        public void CleanUp()
        {
            foreach (var interest in _listOfPlanetoids)
            {
                interest.CleanUp();
            }
        }

        public void Reset()
        {
            foreach (var interest in _listOfPlanetoids)
            {
                interest.Reset();
            }
        }

        private void SetupPlanetoids(PlanetoidBehaviour[] planetoids)
        {
            foreach (var planetoidBehaviour in planetoids)
            {
                planetoidBehaviour.Initialize();
                var planetoid = planetoidBehaviour.PlanetoidModel;
                _listOfPlanetoids.Add(planetoid);
                planetoid.OnActivated += HandleOnInterestActivated;
            }
        }

        private void HandleOnInterestActivated()
        {
            if (_listOfPlanetoids.All(p => p.IsActivated))
            {
                Debug.Log("You Won");
            }
        }
    }
}