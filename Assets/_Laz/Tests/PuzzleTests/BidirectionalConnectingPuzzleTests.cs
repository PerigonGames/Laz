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
    public class BidirectionalConnectingPuzzleTests : InputTestFixture
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
            EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/_Laz/Scenes/TestingScenes/LazBidirectionallPuzzleTests.unity", new LoadSceneParameters(LoadSceneMode.Single));
            _keyboard = InputSystem.AddDevice<Keyboard>();
        }
        
        [UnityTest]
        public IEnumerator Test_BidirectionalPuzzle_DownThenUp_DoorActivates()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            // When
            var _doorToActivate = GameObject.FindObjectOfType<DoorPuzzleActivationBehaviour>();
            _lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();
            
            _lazCoordinatorBehaviour.gameObject.transform.position = Vector3.zero;

            _lazoProperties.DistanceLimitOfLazo = float.MaxValue;
            _lazoProperties.TimeToLivePerPoint = float.MaxValue;
            _lazCoordinatorBehaviour.Initialize(_player, new ILazoWrapped[] { }, _mockMovement, _lazoProperties);


            // Then
            Press(_keyboard.spaceKey);
            
            Press(_keyboard.sKey);
            yield return new WaitForSeconds(1f);
            Release(_keyboard.sKey);
            Press(_keyboard.wKey);
            yield return new WaitForSeconds(1.5f);
            
            Assert.AreEqual(Vector3.zero, _doorToActivate.transform.localScale, "Should Complete Sequential puzzle to hide door");
        }
        
        [UnityTest]
        public IEnumerator Test_BidirectionalPuzzle_OnlyPartlyConnect_DoorDoesNotActivates()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            // When
            var _doorToActivate = GameObject.FindObjectOfType<DoorPuzzleActivationBehaviour>();
            _lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();
            
            _lazCoordinatorBehaviour.gameObject.transform.position = Vector3.zero;

            _lazCoordinatorBehaviour.Initialize(_player, new ILazoWrapped[] { }, _mockMovement, _lazoProperties);


            // Then
            Press(_keyboard.spaceKey);
            Press(_keyboard.sKey);
            yield return new WaitForSeconds(3f);
            
            Assert.AreNotEqual(Vector3.zero, _doorToActivate.transform.localScale, "Should NOT Complete Sequential puzzle to hide door");
        }
    }
}