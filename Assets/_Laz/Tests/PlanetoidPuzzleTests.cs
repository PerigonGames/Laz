using System.Collections;
using Laz;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.Planets
{
    public class PlanetoidPuzzleTests
    {
        [SetUp]
        public void Setup()
        {
            EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/_Laz/Scenes/TestingScenes/PlanetoidPuzzleTests.unity", new LoadSceneParameters(LoadSceneMode.Single));
        }
        
        [UnityTest]
        public IEnumerator Test_PlanetoidPuzzle_ActivatesDoorAOnly()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }
            
            // When
            var planetoid = GameObject.Find("Planetoid_A").GetComponent<PlanetoidBehaviour>();
            
            // Then
            planetoid.PlanetoidModel.ActivateLazo();
            var door = GameObject.Find("Door_A").GetComponent<DoorPuzzleActivationBehaviour>();
            yield return new WaitForFixedUpdate();
            
            // Therefore
            Assert.AreEqual(Vector3.zero, door.transform.localScale, "Door should disappear");
        }
        
        [UnityTest]
        public IEnumerator Test_PlanetoidPuzzle_ActivatesDoorBOnly()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }
            
            // When
            var planetoid_B = GameObject.Find("Planetoid_B").GetComponent<PlanetoidBehaviour>();
            var planetoid_B_2 = GameObject.Find("Planetoid_B_2").GetComponent<PlanetoidBehaviour>();
            var doorA = GameObject.Find("Door_A").GetComponent<DoorPuzzleActivationBehaviour>();
            doorA.gameObject.transform.localScale = Vector3.one;
            
            // Then
            planetoid_B.PlanetoidModel.ActivateLazo();
            planetoid_B_2.PlanetoidModel.ActivateLazo();
            
            var door = GameObject.Find("Door_B").GetComponent<DoorPuzzleActivationBehaviour>();
            yield return new WaitForFixedUpdate();
            
            // Therefore
            Assert.AreEqual(Vector3.zero, door.transform.localScale, "Door B should disappear");
            Assert.AreEqual(Vector3.one, doorA.transform.localScale, "Door A should not disppear");
        }
    }
}

