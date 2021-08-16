using Laz;
using NUnit.Framework;

namespace Tests.Planets
{

    public class LazoWrappableManagerTests
    {
        [Test]
        public void Test_WrappableManager_TriggersWin()
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
        public void Test_WrappableManager_CleanUp()
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
            lazoWrappableManager.CleanUp();

            //Assert
            for (int i = 0; i < numMockPlanets; i++)
            {
                Assert.AreEqual(mockPlanetoids[i].CleanUpCount, 1, "Clean up should be called once for each planet");
            }
        }

        [Test]
        public void Test_WrappableManager_Reset()
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
