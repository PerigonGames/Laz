using Laz;
using NUnit.Framework;

namespace Tests.Planets
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

        [Test]
        public void Test_Planetoid_WrappableManager_TriggersWin()
        {
            //Arrange
            var numMockPlanets = 5;
            MockPlanetoidBehaviour[] mockPlanetoids = new MockPlanetoidBehaviour[numMockPlanets];
            Planetoid[] planetoids = new Planetoid[numMockPlanets];
            for(int i = 0; i < numMockPlanets; i++)
            {
                mockPlanetoids[i] = new MockPlanetoidBehaviour();
                planetoids[i] = new Planetoid(mockPlanetoids[i]);
                mockPlanetoids[i].PlanetoidModel = planetoids[i];
            }
            var mockStateManager = new MockStateManager();
            var lazoWrappableManager = new LazoWrappableManager(mockPlanetoids, mockStateManager);

            //Act
            mockStateManager.SetState(State.Play);
            for (int i = 0; i < numMockPlanets; i++)
            {
                planetoids[i].ActivateLazo();
            }

            //Assert
            Assert.AreEqual(mockStateManager.GetState(), State.WinGame, "State should be set to win");
            for (int i = 0; i < numMockPlanets; i++)
            {
                Assert.IsTrue(planetoids[i].IsActivated, $"Planetoid {i} should be activated");
            }
        }

        [Test]
        public void Test_Planetoid_WrappableManager_CleanUp()
        {
            // Arrange
            var numMockPlanets = 5;
            MockPlanetoidBehaviour[] mockPlanetoids = new MockPlanetoidBehaviour[numMockPlanets];
            Planetoid[] planetoids = new Planetoid[numMockPlanets];
            for (int i = 0; i < numMockPlanets; i++)
            {
                mockPlanetoids[i] = new MockPlanetoidBehaviour();
                planetoids[i] = new Planetoid(mockPlanetoids[i]);
                mockPlanetoids[i].PlanetoidModel = planetoids[i];
            }
            var mockStateManager = new MockStateManager();
            var lazoWrappableManager = new LazoWrappableManager(mockPlanetoids, mockStateManager);

            //Act
            mockStateManager.SetState(State.Play);
            for (int i = 0; i < numMockPlanets; i++)
            {
                planetoids[i].ActivateLazo();
            }
            lazoWrappableManager.CleanUp();

            //Assert
            Assert.AreEqual(mockStateManager.GetState(), State.WinGame, "State should be set to win");
            for (int i = 0; i < numMockPlanets; i++)
            {
                Assert.IsFalse(planetoids[i].IsActivated, $"Planetoid {i} should not be activated");
            }
        }

        [Test]
        public void Test_Planetoid_WrappableManager_Reset()
        {
            //Arrange
            var numMockPlanets = 5;
            MockPlanetoidBehaviour[] mockPlanetoids = new MockPlanetoidBehaviour[numMockPlanets];
            Planetoid[] planetoids = new Planetoid[numMockPlanets];
            for (int i = 0; i < numMockPlanets; i++)
            {
                mockPlanetoids[i] = new MockPlanetoidBehaviour();
                planetoids[i] = new Planetoid(mockPlanetoids[i]);
                mockPlanetoids[i].PlanetoidModel = planetoids[i];
            }
            var mockStateManager = new MockStateManager();
            var lazoWrappableManager = new LazoWrappableManager(mockPlanetoids, mockStateManager);

            //Act
            lazoWrappableManager.Reset();

            //Assert
            for (int i = 0; i < numMockPlanets; i++)
            {
                Assert.AreEqual(mockPlanetoids[i].ResetCount, 1, "Reset should be called once for each planet");
            }

        }

    }

}
