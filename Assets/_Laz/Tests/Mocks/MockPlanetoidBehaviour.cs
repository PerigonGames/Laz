using Laz;
using UnityEngine;

namespace Tests
{
    public class MockPlanetoidBehaviour : IPlanetoidBehaviour
    {
        public int ResetCount = 0;
        public int CleanUpCount = 0;
        
        public void Reset()
        {
            ResetCount++;
        }

        public void CleanUp()
        {
            CleanUpCount++;
        }

        public Planetoid PlanetoidModel { get; set; }

        public void Initialize()
        {
        }

        public void LazoActivated()
        {
        }

        public Vector3 Position { get; }
    }
}