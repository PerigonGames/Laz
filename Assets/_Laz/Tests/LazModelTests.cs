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
    public class LazModelTests : InputTestFixture
    {
        
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
                yield return new WaitForEndOfFrame();
            }

            var lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();
            Press(_keyboard.aKey);

            yield return new WaitForSeconds(0.5f);
            var lazTransforms = lazCoordinatorBehaviour.transform;
            Assert.AreEqual(new Vector3(0, 270, 0), lazTransforms.rotation.eulerAngles, "Pressing Left should rotate to 270 y");
        }
        
        [UnityTest]
        public IEnumerator Test_LazMovement_PressD()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForEndOfFrame();
            }

            var lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();
            Press(_keyboard.dKey);

            yield return new WaitForSeconds(0.5f);
            var lazTransforms = lazCoordinatorBehaviour.transform;
            Assert.AreEqual(new Vector3(0, 90, 0), lazTransforms.rotation.eulerAngles, "Pressing right should rotate to 90 y");
        }
        
        [UnityTest]
        public IEnumerator Test_LazMovement_PressW()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForEndOfFrame();
            }

            var lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();
            Press(_keyboard.wKey);

            yield return new WaitForSeconds(0.5f);
            var lazTransforms = lazCoordinatorBehaviour.transform;
            Assert.AreEqual(new Vector3(0, 0, 0), lazTransforms.rotation.eulerAngles, "Pressing up should rotate to 0 y");
        }
        
        [UnityTest]
        public IEnumerator Test_LazMovement_PressS()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForEndOfFrame();
            }

            var lazCoordinatorBehaviour = GameObject.FindObjectOfType<LazCoordinatorBehaviour>();
            Press(_keyboard.sKey);

            yield return new WaitForSeconds(0.5f);
            var lazTransforms = lazCoordinatorBehaviour.transform;
            Assert.AreEqual(new Vector3(0, 180, 0), lazTransforms.rotation.eulerAngles, "Pressing down should rotate to 180 y");
        }
    }
}

