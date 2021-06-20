using Laz;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class LazoPositionTests
    {
        
        [Test]
        public void TestLazoPosition_ForceDeath()
        {
            // Arrange
            var timeToLive = 100;
            var lazoPosition = new LazoPosition(timeToLive, Vector3.zero);
            var deathCount = 0;
            
            // Act
            lazoPosition.OnTimeBelowZero += () =>
            {
                deathCount++;
            };
            
            lazoPosition.ForceDeath();
            
            // Assert
            Assert.AreEqual(1, deathCount, "should die once");
            Assert.IsTrue(lazoPosition.IsTimeBelowZero);
        }

        [Test]
        public void TestLazoPosition_DecrementTime_Death()
        {
            // Arrange
            var timeToLive = 100;
            var lazoPosition = new LazoPosition(timeToLive, Vector3.back);
            var deathCount = 0;
            
            // Act
            lazoPosition.OnTimeBelowZero += () =>
            {
                deathCount++;
            };
            lazoPosition.DecrementTimeToLiveBy(101);

            
            // Assert
            Assert.AreEqual(1, deathCount, "should die once");
            Assert.IsTrue(lazoPosition.IsTimeBelowZero);
        }
    }
}