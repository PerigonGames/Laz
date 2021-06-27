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
    public class LazBoostTests : InputTestFixture
    {
        private LazPlayer _player = new LazPlayer();           
        private ILazoWrapped[] _dummyWrappableObjects = { };
        private LazCoordinatorBehaviour _lazCoordinatorBehaviour = null;
        private MockLazMovement _mockMovement = new MockLazMovement();
        private MockLazoProperties _lazoProperties = new MockLazoProperties();
        
        private Keyboard _keyboard = null;
        private Mouse _mouse = null;
        
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/_Laz/Scenes/TestingScenes/LazMovementTests.unity", new LoadSceneParameters(LoadSceneMode.Single));
            _keyboard = InputSystem.AddDevice<Keyboard>();
            _mouse = InputSystem.AddDevice<Mouse>();
        }

        [UnityTest]
        public IEnumerator Test_LazBoost_Activated()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }
            var originalPosition = Vector3.zero;
            var expectedBoostSpeed = 30f;
            
            _lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();
            _lazCoordinatorBehaviour.gameObject.transform.position = originalPosition;
            _mockMovement.BoostSpeed = expectedBoostSpeed;
            _mockMovement.Acceleration = 100f;
            _mockMovement.BoostTimeLimit = 100f;
            _lazCoordinatorBehaviour.Initialize(_player, _dummyWrappableObjects, _mockMovement);

            
            Press(_keyboard.aKey);
            Press(_mouse.leftButton);

            yield return new WaitForSeconds(0.5f);
            var actualBoostSpeed = Mathf.Abs(_lazCoordinatorBehaviour.GetComponent<Rigidbody>().velocity.x);
            Assert.AreEqual(expectedBoostSpeed, actualBoostSpeed);
        }
        
        [UnityTest]
        public IEnumerator Test_LazBoostTimeLimit_GoesOver()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }
            var originalPosition = Vector3.zero;
            var expectedLazoSpeed = 11f;

            _lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();
            _lazCoordinatorBehaviour.gameObject.transform.position = originalPosition;
            _mockMovement.BoostSpeed = 20f;
            _mockMovement.BoostTimeLimit = 0.5f;
            _mockMovement.LazoMaxSpeed = expectedLazoSpeed;
            _mockMovement.BaseMaxSpeed = 7f;
            _lazoProperties.DistanceLimitOfLazo = float.MaxValue;
            _lazCoordinatorBehaviour.Initialize(_player, _dummyWrappableObjects, _mockMovement, _lazoProperties);
            
            Press(_keyboard.aKey);
            Press(_mouse.leftButton);

            yield return new WaitForSeconds(2f);
            var actualBoostSpeed = Mathf.Abs(_lazCoordinatorBehaviour.GetComponent<Rigidbody>().velocity.x);
            Assert.AreEqual(expectedLazoSpeed, actualBoostSpeed , "Laz's Speed after boost should go back to Lazo max Speed");
        }
    }
}

