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
    public class LazDeathBoxTests : InputTestFixture
    {
        private LazCoordinatorBehaviour _lazCoordinatorBehaviour = null;
        private Keyboard _keyboard = null;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/_Laz/Scenes/TestingScenes/LazDeathBoxTests.unity", new LoadSceneParameters(LoadSceneMode.Single));
            _keyboard = InputSystem.AddDevice<Keyboard>();
        }
        
        [UnityTest]
        public IEnumerator Test_LazHittingDeathBox()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            // Given
            var mockStates = new MockStateManager();
            mockStates.SetState(State.Play);
            LazPlayer player = new LazPlayer(mockStates);
            
            _lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();
            _lazCoordinatorBehaviour.Initialize(player);
            
            // When
            Press(_keyboard.wKey);

            // Then
            yield return new WaitForSeconds(1.5f);
            Assert.AreEqual(State.Death, mockStates.GetState(), "Should be Death State");
        }
    }
}