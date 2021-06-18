using System.Collections;
using Laz;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class LazoWithPlanetoidCollisionTests : InputTestFixture
    {
        private LazPlayer _player = new LazPlayer();           
        private LazCoordinatorBehaviour _lazCoordinatorBehaviour = null;
        private MockLazMovement _mockMovement = new MockLazMovement();
        private MockLazoProperties _lazoProperties = new MockLazoProperties();
        private Keyboard _keyboard = null;
        
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _mockMovement = new MockLazMovement();
            _lazoProperties = new MockLazoProperties();
            _player = new LazPlayer();  
            EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/_Laz/Scenes/TestingScenes/LazoWithPlanetoidCollisionTests.unity", new LoadSceneParameters(LoadSceneMode.Single));
            _keyboard = InputSystem.AddDevice<Keyboard>();
        }
        
        [UnityTest]
        public IEnumerator Test_LazoWall_CreatedOnLazoActivate()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForEndOfFrame();
            }

            // When
             GameObject.FindObjectOfType<PlanetoidBehaviour>().transform.position = new Vector3(-4, 0, 0);
            _lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();
            var walls = GameObject.FindObjectOfType<LazoWallObjectPooler>();
            walls.Initialize(50);
            
            _lazCoordinatorBehaviour.gameObject.transform.position = new Vector3(0, 0, -4);

            _lazoProperties.DistanceLimitOfLazo = float.MaxValue;
            _lazoProperties.TimeToLivePerPoint = float.MaxValue;
            _lazoProperties.RateOfRecordingPosition = 0.05f;
            _mockMovement.BaseMaxSpeed = 20f;
            _mockMovement.LazoMaxSpeed = 20f;
            _mockMovement.BoostSpeed = 20f;
            _lazCoordinatorBehaviour.Initialize(_player, new ILazoWrapped[] {}, _mockMovement, _lazoProperties);
            
            // Then
            Press(_keyboard.spaceKey);
            
            Press(_keyboard.wKey);
            yield return new WaitForSeconds(1);
            var lazoWalls = GameObject.FindObjectsOfType<LazoWallBehaviour>();

            Assert.GreaterOrEqual(lazoWalls.Length, 1, "Should create more than 1 walls");
        }
    }
}