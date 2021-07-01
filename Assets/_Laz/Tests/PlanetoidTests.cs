using Laz;
using NUnit.Framework;

namespace Tests
{
    public class PlanetoidTests
    {
        [Test]
        public void Test_Planetoid_ActivateLazo()
        {
            // Arrange
            var planetoid = new Planetoid(new MockPlanetoidBehaviour());
            var activatedCalled = false;
            planetoid.OnPlanetoidActivated += () =>
            {
                activatedCalled = true;
            };
            
            // Act
            planetoid.ActivateLazo();
            
            //Assert
            Assert.IsTrue(activatedCalled, "Subscriber should be called");
            Assert.IsTrue(planetoid.IsActivated, "Planetoid should be activated");
        }
        
        [Test]
        public void Test_Planetoid_CleanUp()
        {
            // Arrange
            var planetoid = new Planetoid(new MockPlanetoidBehaviour());
            var activatedCalled = false;
            planetoid.OnPlanetoidActivated += () =>
            {
                activatedCalled = true;
            };
            
            // Act
            planetoid.ActivateLazo();
            planetoid.CleanUp();
            
            //Assert
            Assert.IsTrue(activatedCalled, "Subscriber should be called");
            Assert.IsFalse(planetoid.IsActivated, "Planetoid should not be activated");
        }
        
        [Test]
        public void Test_Planetoid_Reset()
        {
            // Arrange
            var mockPlanetoidBehaviour = new MockPlanetoidBehaviour();
            var planetoid = new Planetoid(mockPlanetoidBehaviour);

            // Act
            planetoid.Reset();
            
            //Assert
            Assert.AreEqual(mockPlanetoidBehaviour.ResetCount, 1, "Reset should be called once");
        }

    }

}
