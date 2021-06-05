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
    public class LazoBehaviourTests : InputTestFixture
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
            EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/_Laz/Scenes/TestingScenes/LazoTests.unity", new LoadSceneParameters(LoadSceneMode.Single));
            _keyboard = InputSystem.AddDevice<Keyboard>();
        }

        [UnityTest]
        public IEnumerator Test_Lazo_Planetoid_Activate()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForEndOfFrame();
            }

            // When
            var worldManager = GameObject.FindObjectOfType<MockWorldManager>();
            var planetoid = GameObject.FindObjectOfType<PlanetoidBehaviour>();

            var dummyWrappableObjects = new[] { worldManager.Wrappable };
            _lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();
            
            planetoid.gameObject.transform.position = new Vector3(-5, 0, 0);
            _lazCoordinatorBehaviour.gameObject.transform.position = new Vector3(5, 0, 0);

            _lazoProperties.DistanceLimitOfLazo = float.MaxValue;
            _lazoProperties.TimeToLivePerPoint = float.MaxValue;
            _mockMovement.BaseMaxSpeed = 20f;
            _mockMovement.LazoMaxSpeed = 20f;
            _mockMovement.BoostSpeed = 20f;
            _lazCoordinatorBehaviour.Initialize(_player, dummyWrappableObjects);
            var lazo = new Lazo(_lazoProperties, dummyWrappableObjects, new MockBoost());
            _player.SetLazo(_lazoProperties, dummyWrappableObjects, new MockBoost());
            _player.SetMovement(_mockMovement, lazo);


            // Then
            Press(_keyboard.spaceKey);
            
            Press(_keyboard.wKey);
            yield return new WaitForSeconds(0.25f);
            Release(_keyboard.wKey);
            
            Press(_keyboard.aKey);
            yield return new WaitForSeconds(1f);
            Release(_keyboard.aKey);
            
            Press(_keyboard.sKey);
            yield return new WaitForSeconds(0.5f);
            Release(_keyboard.sKey);
            
            Press(_keyboard.dKey);
            yield return new WaitForSeconds(1f);
            Release(_keyboard.dKey);
            
            Press(_keyboard.wKey);
            yield return new WaitForSeconds(1f);
            
            Assert.IsTrue(planetoid.IsActivated);
        }
        
        [UnityTest]
        public IEnumerator Test_Lazo_EmptySpace()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForEndOfFrame();
            }

            // When
            var worldManager = GameObject.FindObjectOfType<MockWorldManager>();
            var planetoid = GameObject.FindObjectOfType<PlanetoidBehaviour>();

            var dummyWrappableObjects = new[] { worldManager.Wrappable };
            _lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();
            
            planetoid.gameObject.transform.position = new Vector3(-5, 0, 0);
            _lazCoordinatorBehaviour.gameObject.transform.position = new Vector3(5, 0, 0);

            _lazoProperties.DistanceLimitOfLazo = float.MaxValue;
            _lazoProperties.TimeToLivePerPoint = float.MaxValue;
            _mockMovement.BaseMaxSpeed = 20f;
            _mockMovement.LazoMaxSpeed = 20f;
            _mockMovement.BoostSpeed = 20f;
            _lazCoordinatorBehaviour.Initialize(_player, dummyWrappableObjects);
            var lazo = new Lazo(_lazoProperties, dummyWrappableObjects, new MockBoost());
            _player.SetLazo(_lazoProperties, dummyWrappableObjects, new MockBoost());
            _player.SetMovement(_mockMovement, lazo);


            // Then
            Press(_keyboard.spaceKey);
            
            Press(_keyboard.wKey);
            yield return new WaitForSeconds(0.25f);
            Release(_keyboard.wKey);
            
            Press(_keyboard.dKey);
            yield return new WaitForSeconds(1f);
            Release(_keyboard.dKey);
            
            Press(_keyboard.sKey);
            yield return new WaitForSeconds(0.5f);
            Release(_keyboard.sKey);
            
            Press(_keyboard.aKey);
            yield return new WaitForSeconds(1f);
            Release(_keyboard.aKey);
            
            Press(_keyboard.wKey);
            yield return new WaitForSeconds(1f);
            
            Assert.IsFalse(planetoid.IsActivated);
        }
    }
    

}