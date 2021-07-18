using Laz;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Utilities
{
    public class RandomPositionInsideCircleTests
    {
        [Test]
        public void TestGetRandomPosition_RandomizerAlwaysReturns0_ReturnsSpawnPosition()
        {
            // Arrange
            var radius = 4;
            var startingPosition = new Vector3(2, 3, 4);
            var mockRandomUtility = new MockRandomUtility();
            mockRandomUtility.MockDouble = 0;
            
            var randomPositionInsideCircle = new RandomPositionInsideCircle(startingPosition, radius, mockRandomUtility);
            
            // Act
            var actualResult = randomPositionInsideCircle.GetRandomPosition();
            
            // Assert
            Assert.AreEqual(new Vector3(2, 3, 4), actualResult, "Randomizer should always return 0, so position returned should be starting position");
        }
        
        [Test]
        public void TestGetRandomPosition_RandomizerReturnsQuarter_ReturnQuarterDistanceAwayFromCenter()
        {
            // Arrange
            var radius = 40;
            var startingPosition = Vector3.zero;
            var mockRandomUtility = new MockRandomUtility();
            mockRandomUtility.MockDouble = 0.25;
            var expectedResult = new Vector3(0, 0, 10f);
            
            var randomPositionInsideCircle = new RandomPositionInsideCircle(startingPosition, radius, mockRandomUtility);
            
            // Act
            var actualResult = randomPositionInsideCircle.GetRandomPosition();

            // Assert
            /// Floating Point Error causes hard to test whole Vector3
            /// So separated tests to test each axis
            Assert.AreEqual(expectedResult.x, actualResult.x, "Randomizer always return 0.25, So Quarter angle circle + Quarter distance from center");
            Assert.AreEqual(expectedResult.y, actualResult.y, "Randomizer always return 0.25, So Quarter angle circle + Quarter distance from center");
            Assert.AreEqual(expectedResult.z, actualResult.z, "Randomizer always return 0.25, So Quarter angle circle + Quarter distance from center");
        }
    }
}
