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
                yield return new WaitForEndOfFrame();
            }
            var originalPosition = Vector3.zero;
            var expectedBoostSpeed = 30f;

            _lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();
            _lazCoordinatorBehaviour.gameObject.transform.position = originalPosition;
            _mockMovement.BoostSpeed = expectedBoostSpeed;
            _mockMovement.Acceleration = 100f;
            _mockMovement.BoostTimeLimit = 100f;
            _player.Set(_mockMovement, new Lazo(_lazoProperties, _dummyWrappableObjects));
            _lazCoordinatorBehaviour.Initialize(_player, _dummyWrappableObjects);
            
            Press(_keyboard.aKey);
            Press(_mouse.leftButton);

            yield return new WaitForSeconds(1.5f);
            Assert.AreEqual(_lazCoordinatorBehaviour.GetComponent<Rigidbody>().velocity, expectedBoostSpeed);
        }
    }
}

