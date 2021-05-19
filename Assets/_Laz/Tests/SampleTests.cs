using System.Collections;
using Laz;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class MockPlanetoidBehaviour : IPlanetoidBehaviour
    {
        public void Reset()
        {
            throw new System.NotImplementedException();
        }

        public void CleanUp()
        {
            throw new System.NotImplementedException();
        }

        public Planetoid PlanetoidModel { get; }
        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public void LazoActivated()
        {
            throw new System.NotImplementedException();
        }

        public Vector3 Position { get; }
    }
    
    public class SampleTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void SampleTests1SimplePasses()
        {
            Planetoid planetoid = new Planetoid(new MockPlanetoidBehaviour());
            // Use the Assert class to test conditions
            Assert.True(true);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator SampleTests1WithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    } 
}

