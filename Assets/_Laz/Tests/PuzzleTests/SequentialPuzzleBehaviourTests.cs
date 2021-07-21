using System.Collections;
using Laz;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.Puzzles
{
    public class SequentialPuzzleBehaviourTests : InputTestFixture
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
            EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/_Laz/Scenes/TestingScenes/LazSequentialPuzzleTests.unity", new LoadSceneParameters(LoadSceneMode.Single));
            _keyboard = InputSystem.AddDevice<Keyboard>();
        }
        
        [UnityTest]
        public IEnumerator Test_SequentialPuzzle_DoorActivates()
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
            _lazoProperties.RateOfRecordingPosition = 0.05f;
            _lazCoordinatorBehaviour.Initialize(_player, new ILazoWrapped[] { }, _mockMovement, _lazoProperties);


            // Then
            Press(_keyboard.spaceKey);
            
            Press(_keyboard.sKey);
            yield return new WaitForSeconds(2f);
            
            Assert.AreEqual(Vector3.zero, _doorToActivate.transform.localScale, "Should Complete Sequential puzzle to hide door");
        }
        
        [UnityTest]
        public IEnumerator Test_SequentialPuzzle_LineRenderers_Generated()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            // When
            _lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();
            
            _lazCoordinatorBehaviour.gameObject.transform.position = Vector3.zero;

            _lazoProperties.DistanceLimitOfLazo = float.MaxValue;
            _lazoProperties.TimeToLivePerPoint = float.MaxValue;
            _lazoProperties.RateOfRecordingPosition = 0.05f;
            _lazCoordinatorBehaviour.Initialize(_player, new ILazoWrapped[] { }, _mockMovement, _lazoProperties);
            
            // Then
            Press(_keyboard.spaceKey);
            Press(_keyboard.sKey);
            yield return new WaitForSeconds(2f);

            var sequentialPuzzle = Object.FindObjectOfType<SequentialConnectingPuzzleBehaviour>();
            var lineRenderers = sequentialPuzzle.GetComponentsInChildren<LineRenderer>();
            Assert.AreEqual(3, lineRenderers.Length, "Should have 3 line renderers generated");
        }
        
        [UnityTest]
        public IEnumerator Test_SequentialPuzzle_CleanUp_RemoveLineRenderer()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            // When
            _lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();
            
            _lazCoordinatorBehaviour.gameObject.transform.position = Vector3.zero;

            _lazoProperties.DistanceLimitOfLazo = float.MaxValue;
            _lazoProperties.TimeToLivePerPoint = float.MaxValue;
            _lazCoordinatorBehaviour.Initialize(_player, new ILazoWrapped[] { }, _mockMovement, _lazoProperties);


            // Then
            Press(_keyboard.spaceKey);
            
            Press(_keyboard.sKey);
            yield return new WaitForSeconds(2f);
            Release(_keyboard.spaceKey);
            Release(_keyboard.sKey);
            var sequentialPuzzle = Object.FindObjectOfType<SequentialConnectingPuzzleBehaviour>();
            sequentialPuzzle.CleanUp();
            yield return new WaitForSeconds(0);

            // Therefore
            var lineRenderers = sequentialPuzzle.GetComponentsInChildren<LineRenderer>();
            Assert.AreEqual(0, lineRenderers.Length, "All Line Renderers should be cleaned up and gone");
        }
        
        [UnityTest]
        public IEnumerator Test_SequentialPuzzle_DoorDoesNotActivates()
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
            Press(_keyboard.sKey);
            yield return new WaitForSeconds(2f);
            
            Assert.AreNotEqual(Vector3.zero, _doorToActivate.transform.localScale, "Should NOT Complete Sequential puzzle to hide door");
        }
        
        [UnityTest]
        public IEnumerator Test_SequentialPuzzle_OnlyPartlyConnect_DoorDoesNotActivates()
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
            yield return new WaitForSeconds(0.5f);
            Release(_keyboard.spaceKey);
            yield return new WaitForSeconds(1f);
            
            Assert.AreNotEqual(Vector3.zero, _doorToActivate.transform.localScale, "Should NOT Complete Sequential puzzle to hide door");
        }
        
        [UnityTest]
        public IEnumerator Test_SequentialPuzzle_ReverseSolve_DoorDoesNotActivate()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            // When
            var doorToActivate = GameObject.FindObjectOfType<DoorPuzzleActivationBehaviour>();
            var expectedScale = new Vector3(18, 1.5f, 1f);
            doorToActivate.transform.localScale = expectedScale;
            _lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();

            _lazCoordinatorBehaviour.gameObject.transform.position = new Vector3(0, 0, -15f);
            _lazoProperties.RateOfRecordingPosition = 0.05f;
            _lazCoordinatorBehaviour.Initialize(_player, new ILazoWrapped[] { }, _mockMovement, _lazoProperties);


            // Then
            Press(_keyboard.spaceKey);
            Press(_keyboard.wKey);
            yield return new WaitForSeconds(3f);
            
            Assert.AreEqual(expectedScale, doorToActivate.transform.localScale, "Should NOT Complete Sequential puzzle to hide door");
        }
    }
}