using Laz;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Patrolling
{
    public class PatrolTests
    {
        [Test]
        public void Test_Patrol_Constructor()
        {
            //Arrange
            var startingPosition = new Vector3(1, 0, 3);
            var firstPatrolPosition = new Vector3(-1, 0, -3);
            var secondPatrolPosition = new Vector3(2, 0, 5);
            Vector3[] patrolLocation = {firstPatrolPosition, secondPatrolPosition};
            var speed = 5;
            
            // Act
            var patrol = new Patrol(startingPosition, patrolLocation, speed);
            
            // Assert
            Assert.AreEqual(startingPosition, patrol.StartingLocation, "Starting Location should be the original starting position of transform.position");
            Assert.AreEqual(firstPatrolPosition, patrol.CurrentDestination, "Should queue up the first item in the array first");
        }
        
        [Test]
        public void Test_Patrol_CleanUp()
        {
            //Arrange
            var startingPosition = new Vector3(1, 0, 3);
            var firstPatrolPosition = new Vector3(-1, 0, -3);
            var secondPatrolPosition = new Vector3(2, 0, 5);
            Vector3[] patrolLocation = {firstPatrolPosition, secondPatrolPosition};
            var speed = 5;
            var patrol = new Patrol(startingPosition, patrolLocation, speed);
            
            //Act
            patrol.CleanUp();
            
            // Assert
            Assert.IsNull(patrol.CurrentDestination, "Current destination should be null after cleanup");
        }
        
        [Test]
        public void Test_Patrol_Reset()
        {
            //Arrange
            var startingPosition = new Vector3(1, 0, 3);
            var firstPatrolPosition = new Vector3(-1, 0, -3);
            var secondPatrolPosition = new Vector3(2, 0, 5);
            Vector3[] patrolLocation = {firstPatrolPosition, secondPatrolPosition};
            var speed = 5;
            var patrol = new Patrol(startingPosition, patrolLocation, speed);
            
            // Act
            patrol.Reset();
            
            // Assert
            Assert.AreEqual(startingPosition, patrol.StartingLocation, "Starting Location should be the original starting position of transform.position");
            Assert.AreEqual(firstPatrolPosition, patrol.CurrentDestination, "Should queue up the first item in the array first");
        }
        
        [Test]
        public void Test_Patrol_MoveTowards_ChangedDestination()
        {
            //Arrange
            var startingPosition = new Vector3(1, 0, 3);
            var firstPatrolPosition = new Vector3(-1, 0, -3);
            var secondPatrolPosition = new Vector3(2, 0, 5);
            Vector3[] patrolLocation = {firstPatrolPosition, secondPatrolPosition};
            var speed = 5;
            var patrol = new Patrol(startingPosition, patrolLocation, speed);
            
            // Act
            patrol.MoveTowards(Vector3.zero, 100);
            
            // Assert
            Assert.AreEqual(secondPatrolPosition, patrol.CurrentDestination, "Should queue up the first item in the array first");
        }
        
        [Test]
        public void Test_Patrol_MoveTowards_StillSameDestination()
        {
            //Arrange
            var startingPosition = new Vector3(1, 0, 3);
            var firstPatrolPosition = new Vector3(-1, 0, -3);
            var secondPatrolPosition = new Vector3(2, 0, 5);
            Vector3[] patrolLocation = {firstPatrolPosition, secondPatrolPosition};
            var speed = 5;
            var patrol = new Patrol(startingPosition, patrolLocation, speed);
            
            // Act
            patrol.MoveTowards(Vector3.zero, 0.5f);
            
            // Assert
            Assert.AreEqual(firstPatrolPosition, patrol.CurrentDestination, "Should queue up the first item in the array first");
        }
    }
}

