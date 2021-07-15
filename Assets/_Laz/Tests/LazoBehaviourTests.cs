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
                yield return new WaitForFixedUpdate();
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
            _lazCoordinatorBehaviour.Initialize(_player, dummyWrappableObjects, _mockMovement, _lazoProperties);


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
            yield return new WaitForSeconds(0.9f);
            Release(_keyboard.dKey);
            
            Press(_keyboard.wKey);
            yield return new WaitForSeconds(1f);
            
            Assert.IsTrue(planetoid.IsPuzzleActivated);
        }
        
        [UnityTest]
        public IEnumerator Test_Lazo_EmptySpace()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
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
            _lazCoordinatorBehaviour.Initialize(_player, dummyWrappableObjects, _mockMovement, _lazoProperties);

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
            
            Assert.IsFalse(planetoid.IsPuzzleActivated);
        }
        
        [UnityTest]
        public IEnumerator Test_Lazo_IsLazoingLimited()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            // When
            var worldManager = GameObject.FindObjectOfType<MockWorldManager>();
            var planetoid = GameObject.FindObjectOfType<PlanetoidBehaviour>();

            var dummyWrappableObjects = new[] { worldManager.Wrappable };
            _lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();
            
            planetoid.gameObject.transform.position = new Vector3(-5, 0, 0);
            _lazCoordinatorBehaviour.gameObject.transform.position = new Vector3(5, 0, 0);

            _lazoProperties.DistanceLimitOfLazo = 0.5f;
            _mockMovement.BaseMaxSpeed = 1;
            _mockMovement.LazoMaxSpeed = 1;
            _mockMovement.BoostSpeed = 1;
            _lazCoordinatorBehaviour.Initialize(_player, dummyWrappableObjects, _mockMovement, _lazoProperties);
            
            // Then
            Press(_keyboard.spaceKey);
            
            Press(_keyboard.wKey);
            yield return new WaitForSeconds(2f);
            
            Assert.IsFalse(_player.LazoTool.IsLazoing);
        }
        
        [UnityTest]
        public IEnumerator Test_LazoReset()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            // When
            var worldManager = GameObject.FindObjectOfType<MockWorldManager>();
            var planetoid = GameObject.FindObjectOfType<PlanetoidBehaviour>();

            var dummyWrappableObjects = new[] { worldManager.Wrappable };
            _lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();
            
            planetoid.gameObject.transform.position = new Vector3(-5, 0, 0);
            _lazCoordinatorBehaviour.gameObject.transform.position = new Vector3(5, 0, 0);

            _lazoProperties.DistanceLimitOfLazo = 5;
            _mockMovement.Acceleration = 1f;
            _lazCoordinatorBehaviour.Initialize(_player, dummyWrappableObjects, _mockMovement, _lazoProperties);
            var lazoBehaviour = _lazCoordinatorBehaviour.GetComponent<LazoBehaviour>();


            // Then
            Press(_keyboard.spaceKey);
            
            Press(_keyboard.wKey);
            yield return new WaitForSeconds(0.1f);
            lazoBehaviour.ResetLazoLimit();
            yield return new WaitForSeconds(0.1f);
            lazoBehaviour.ResetLazoLimit();
            yield return new WaitForSeconds(0.1f);
            lazoBehaviour.ResetLazoLimit();
            yield return new WaitForSeconds(0.1f);
            lazoBehaviour.ResetLazoLimit();
            yield return new WaitForFixedUpdate();
            
            Assert.IsTrue(_player.LazoTool.IsLazoing);
        }
        
        [UnityTest]
        public IEnumerator Test_LazoWallObjectPooler_Initializer()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            // When
            var amountToSpawn = 11;
            var lazoWallObjectPool = GameObject.FindObjectOfType<LazoWallObjectPooler>();
            lazoWallObjectPool.Initialize(amountToSpawn);
            yield return new WaitForFixedUpdate();

            // Then
            var actualResult = lazoWallObjectPool.gameObject.transform.childCount;
            
            //Therefore
            Assert.AreEqual(11, actualResult, "Amount spawned should be 11");
        }
    }
    

}