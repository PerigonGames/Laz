using System.Collections;
using Laz;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.LazCharacter
{
    public class LazMovementsBehaviourTests : InputTestFixture
    {
        private LazPlayer _player = new LazPlayer();           
        private ILazoWrapped[] _dummyWrappableObjects = { };
        private LazCoordinatorBehaviour _lazCoordinatorBehaviour = null;
        private MockLazMovement _mockMovement = new MockLazMovement();
        private MockLazoProperties _lazoProperties = new MockLazoProperties();

        private Keyboard _keyboard = null;
        
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/_Laz/Scenes/TestingScenes/LazMovementTests.unity", new LoadSceneParameters(LoadSceneMode.Single));
            _keyboard = InputSystem.AddDevice<Keyboard>();
        }
        
        [UnityTest]
        public IEnumerator Test_LazMovement_PressA()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            _lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();
            var originalPosition = Vector3.zero;
            _lazCoordinatorBehaviour.gameObject.transform.position = originalPosition;
            _player.SetMovement(_mockMovement, new Lazo(_lazoProperties, _dummyWrappableObjects, new MockBoost()));
            _lazCoordinatorBehaviour.Initialize(_player, _dummyWrappableObjects);
            Press(_keyboard.aKey);

            yield return new WaitForSeconds(0.5f);
            
            Assert.Less(_lazCoordinatorBehaviour.gameObject.transform.position.x, originalPosition.x, "Position should be on the left hand side when pressing a");
        }
        
        [UnityTest]
        public IEnumerator Test_LazMovement_PressD()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            // Given
            _lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();
            var originalPosition = Vector3.zero;
            _lazCoordinatorBehaviour.gameObject.transform.position = originalPosition;
            _player.SetMovement(_mockMovement, new Lazo(_lazoProperties, _dummyWrappableObjects, new MockBoost()));
            _lazCoordinatorBehaviour.Initialize(_player, _dummyWrappableObjects);
            
            // When
            Press(_keyboard.dKey);

            // Then
            yield return new WaitForSeconds(0.5f);
            Assert.Greater(_lazCoordinatorBehaviour.gameObject.transform.position.x, originalPosition.x, "Position should be on the left hand side when pressing a");
        }
        
        [UnityTest]
        public IEnumerator Test_LazMovement_PressW()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            // Given
            _lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();
            var originalPosition = Vector3.zero;
            _lazCoordinatorBehaviour.gameObject.transform.position = originalPosition;
            _player.SetMovement(_mockMovement, new Lazo(_lazoProperties, _dummyWrappableObjects, new MockBoost()));
            _lazCoordinatorBehaviour.Initialize(_player, _dummyWrappableObjects);
            
            // When
            Press(_keyboard.wKey);

            // Then
            yield return new WaitForSeconds(0.5f);
            Assert.Greater(_lazCoordinatorBehaviour.gameObject.transform.position.z, originalPosition.z, "Position should be on the left hand side when pressing a");
        }
        
        [UnityTest]
        public IEnumerator Test_LazMovement_PressS()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            // Given
            _lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();
            var originalPosition = Vector3.zero;
            _lazCoordinatorBehaviour.gameObject.transform.position = originalPosition;
            _player.SetMovement(_mockMovement, new Lazo(_lazoProperties, _dummyWrappableObjects, new MockBoost()));
            _lazCoordinatorBehaviour.Initialize(_player, _dummyWrappableObjects);
            
            // When
            Press(_keyboard.sKey);
            yield return new WaitForSeconds(0.5f);
            
            // Then
            Assert.Less(_lazCoordinatorBehaviour.gameObject.transform.position.z, originalPosition.z, "Position should be on the left hand side when pressing a");
        }
        
        [UnityTest]
        public IEnumerator Test_LazMovement_BaseSpeed()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            // Given
            var maxBaseSpeed = 12f;            
            var originalPosition = Vector3.zero;
            _lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();
            _lazCoordinatorBehaviour.gameObject.transform.position = originalPosition;
            _mockMovement.BaseMaxSpeed = maxBaseSpeed;
            _lazCoordinatorBehaviour.Initialize(_player, _dummyWrappableObjects, _mockMovement);
            
            // When
            Press(_keyboard.aKey);
            yield return new WaitForSeconds(0.5f);
            
            var actualBaseSpeed = Mathf.Abs(_lazCoordinatorBehaviour.GetComponent<Rigidbody>().velocity.x);

            // Then
            Assert.AreEqual(actualBaseSpeed, maxBaseSpeed, "Base speed should be 12f");
        }
        
        [UnityTest]
        public IEnumerator Test_LazMovement_PressWASD_ShouldNotChangeYAxis()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            // Given
            _lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();
            var originalPosition = Vector3.zero;
            _lazCoordinatorBehaviour.gameObject.transform.position = originalPosition;
            _player.SetMovement(_mockMovement, new Lazo(_lazoProperties, _dummyWrappableObjects, new MockBoost()));
            _lazCoordinatorBehaviour.Initialize(_player, _dummyWrappableObjects);
            
            // When
            Press(_keyboard.wKey);
            yield return new WaitForSeconds(0.5f);
            Press(_keyboard.aKey);
            yield return new WaitForSeconds(0.5f);
            Press(_keyboard.sKey);
            yield return new WaitForSeconds(0.5f);
            Press(_keyboard.dKey);
            yield return new WaitForSeconds(0.5f);
            
            // Then
            Assert.AreEqual(_lazCoordinatorBehaviour.gameObject.transform.position.y, originalPosition.y, "Y Position should not change");
        }
    }




}

