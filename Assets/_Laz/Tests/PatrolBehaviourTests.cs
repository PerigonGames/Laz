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
    public class PatrolBehaviourTests : InputTestFixture
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/_Laz/Scenes/TestingScenes/PatrolTests.unity", new LoadSceneParameters(LoadSceneMode.Single));
        }
        
        [UnityTest]
        public IEnumerator Test_PatrolMovement_ShouldMove()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            var patrollingObject = GameObject.FindObjectOfType<PatrolBehaviour>();
            var originalPosition = patrollingObject.transform.position;

            yield return new WaitForSeconds(1);
            Assert.AreNotEqual(originalPosition, patrollingObject.transform.position, "Patrolling object should have moved");
        }
        
        [UnityTest]
        public IEnumerator Test_PatrolMovement_CleanUp()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            var patrollingObject = GameObject.FindObjectOfType<PatrolBehaviour>();
            patrollingObject.gameObject.transform.position = Vector3.left;
            patrollingObject.Initialize();
            patrollingObject.CleanUp();
                
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(Vector3.zero, patrollingObject.transform.position, "Patrolling object should have moved");
        }
        
        [UnityTest]
        public IEnumerator Test_PatrolMovement_Reset()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            var patrollingObject = GameObject.FindObjectOfType<PatrolBehaviour>();
            patrollingObject.gameObject.transform.position = Vector3.left;
            patrollingObject.Initialize();
            patrollingObject.Reset();
                
            yield return new WaitForSeconds(0);
            Assert.AreEqual(Vector3.left, patrollingObject.transform.position, "Patrolling object should have moved");
        }
    }
}

