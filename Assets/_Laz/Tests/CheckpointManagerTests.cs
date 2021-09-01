using Laz;
using NUnit.Framework;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace Tests.CheckPoints
{
    public class CheckPointManagerTests : InputTestFixture
    {
        private Vector3 _initialCheckpointPosition = new Vector3(-5f, 0f, 0f);
        private Vector3 _newCheckPointPosition = new Vector3(5f, 0f, 0f);

        private LazPlayer _player = new LazPlayer();
        private CheckPointManager _checkPointManager = null;
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
            EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/_Laz/Scenes/TestingScenes/LazCheckPointManagerTests.unity", new LoadSceneParameters(LoadSceneMode.Single));
            _keyboard = InputSystem.AddDevice<Keyboard>();
        }

        [UnityTest]
        public IEnumerator Test_CheckPointManager_TestInitialize()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            // When
            _checkPointManager = GameObject.FindObjectOfType<CheckPointManager>();

            // Then
            yield return new WaitForSeconds(0);
            _checkPointManager.Initialize(_player);

            // Therefore
            
            var actualSpawnPosition = _player.SpawnPosition;
            Assert.AreEqual(_initialCheckpointPosition, actualSpawnPosition, "Spawn position is not set to initial checkpoint position!");
        }

        [UnityTest]
        public IEnumerator Test_CheckPointManager_TestSetNewCheckpoint()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            // When
            _lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();
            _checkPointManager = GameObject.FindObjectOfType<CheckPointManager>();
            _lazCoordinatorBehaviour.gameObject.transform.position = Vector3.zero;

            _lazoProperties.DistanceLimitOfLazo = float.MaxValue;
            _lazoProperties.TimeToLivePerPoint = float.MaxValue;
            _lazCoordinatorBehaviour.Initialize(_player, new ILazoWrapped[] { }, _mockMovement, _lazoProperties);
            _checkPointManager.Initialize(_player);

            // Then
            Press(_keyboard.dKey);
            yield return new WaitForSeconds(1.5f);
            Release(_keyboard.dKey);
            yield return new WaitForSeconds(0);

            // Therefore
            var actualSpawnPosition = _player.SpawnPosition;
            Assert.AreEqual(_newCheckPointPosition, actualSpawnPosition, "Spawn position is not set to new checkpoint position!");
        }

        [UnityTest]
        public IEnumerator Test_CheckPointManager_TestResetOldCheckpoint()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            // When
            _lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();
            _checkPointManager = GameObject.FindObjectOfType<CheckPointManager>();
            _lazCoordinatorBehaviour.gameObject.transform.position = Vector3.zero;

            _lazoProperties.DistanceLimitOfLazo = float.MaxValue;
            _lazoProperties.TimeToLivePerPoint = float.MaxValue;
            _lazCoordinatorBehaviour.Initialize(_player, new ILazoWrapped[] { }, _mockMovement, _lazoProperties);
            _checkPointManager.Initialize(_player);

            // Then
            Press(_keyboard.dKey);
            yield return new WaitForSeconds(1.5f);
            Release(_keyboard.dKey);
            yield return new WaitForSeconds(0);
            Press(_keyboard.aKey);
            yield return new WaitForSeconds(3f);
            Release(_keyboard.aKey);

            // Therefore
            var actualSpawnPosition = _player.SpawnPosition;
            Assert.AreEqual(_initialCheckpointPosition, actualSpawnPosition, "Spawn position is not returning to the old checkpoint position!");
        }
    }
}
