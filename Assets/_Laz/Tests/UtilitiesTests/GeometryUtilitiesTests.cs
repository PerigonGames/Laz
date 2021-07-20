using Laz;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Utilities
{
    public class GeometryUtilitiesTests
    {
        [Test]
        public void Test_IsIntersecting_ShouldIntersect()
        {
            // Arrange
            var point1 = new Vector3(0, 0, 0);
            var point2 = new Vector3(5, 0, 0);
            var otherPoint1 = new Vector3(2, 0, 1);
            var otherPoint2 = new Vector3(2, 0, -1);

            // Act
            var actualResult = GeometryUtilities.IsIntersecting(point1, point2, otherPoint1, otherPoint2);

            // Assert
            Assert.IsTrue(actualResult, "Perpendicular intersection");
        }

        [Test]
        public void Test_IsIntersectingOverlappingParallelLine_ShouldNotIntersect()
        {
            // Arrange
            var point1 = new Vector3(0, 0, 0);
            var point2 = new Vector3(5, 0, 0);
            var otherPoint1 = new Vector3(10, 0, 0);
            var otherPoint2 = new Vector3(2, 0, 0);

            //Act
            var actualResult = GeometryUtilities.IsIntersecting(point1, point2, otherPoint1, otherPoint2);

            // Assert
            Assert.IsFalse(actualResult, "Parallel overlapping line won't be closed loop");
        }

        [Test]
        public void Test_IsIntersecting_ShouldNotIntersect()
        {
            // Arrange
            var point1 = new Vector3(0, 0, 0);
            var point2 = new Vector3(5, 0, 0);
            var otherPoint1 = new Vector3(0, 0, -1);
            var otherPoint2 = new Vector3(5, 0, -1);

            //Act
            var actualResult = GeometryUtilities.IsIntersecting(point1, point2, otherPoint1, otherPoint2);

            //Assert
            Assert.IsFalse(actualResult, "the lines should not intersect");
        }

        [Test]
        public void Test_CenterPoint()
        {
            var allPoints = new[]
            {
                new Vector3(0, 0, 0),
                new Vector3(4, 0, 0),
                new Vector3(0, 0, 4),
                new Vector3(4, 0, 4)
            };

            //Act
            var actualResult = GeometryUtilities.CenterPoint(allPoints);

            //Assert
            Assert.AreEqual(new Vector3(2, 0, 2), actualResult, "Center should be 2, 0, 2");
        }

        [Test]
        public void Test_IsInside_PointIsInside()
        {
            var allPoints = new[]
            {
                new Vector3(0, 0, 0),
                new Vector3(4, 0, 0),
                new Vector3(0, 0, 4),
                new Vector3(4, 0, 4)
            };
            
            // Act
            var actualResult = GeometryUtilities.IsInside(allPoints, new Vector3(2, 0, 2));
            
            // Assert
            Assert.IsTrue(actualResult);
        }
        
        [Test]
        public void Test_IsInside_PointIsOutside()
        {
            var allPoints = new[]
            {
                new Vector3(0, 0, 0),
                new Vector3(4, 0, 0),
                new Vector3(0, 0, 4),
                new Vector3(4, 0, 4)
            };
            
            // Act
            var actualResult = GeometryUtilities.IsInside(allPoints, new Vector3(109, 0, 2));
            
            // Assert
            Assert.IsFalse(actualResult);
        }
        
        [Test]
        public void Test_IsInside_PointOnLine()
        {
            var allPoints = new[]
            {
                new Vector3(0, 0, 0),
                new Vector3(4, 0, 0),
                new Vector3(0, 0, 4),
                new Vector3(4, 0, 4)
            };
            
            // Act
            var actualResult = GeometryUtilities.IsInside(allPoints, new Vector3(4, 0, 1));
            
            // Assert
            Assert.IsFalse(actualResult);
        }
    }
}
