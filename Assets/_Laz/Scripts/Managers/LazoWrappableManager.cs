using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Laz
{
    public class LazoWrappableManager: ILifeCycle
    {
        private readonly List<Planetoid> _listOfPlanetoids = new List<Planetoid>();
        private readonly IStateManager _stateManager = null;
        public ILazoWrapped[] WrappableObjects { get; } = null;

        public LazoWrappableManager(IPlanetoidBehaviour[] planetoids, IStateManager stateManager)
        {
            _stateManager = stateManager;
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

        private void SetupPlanetoids(IPlanetoidBehaviour[] planetoids)
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
                _stateManager.SetState(State.WinGame);
            }
        }
    }
}