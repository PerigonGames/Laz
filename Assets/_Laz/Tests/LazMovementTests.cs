using Laz;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class LazMovementTests
    {
        private MockLazMovement _mockLazMovement = new MockLazMovement();
        private Lazo _lazo;
        [SetUp]
        public void Setup()
        {
            _mockLazMovement = new MockLazMovement();
            _lazo = new Lazo(new MockLazoProperties(), new ILazoWrapped[]{}, new MockBoost());
        }

        [Test]
        public void Test_LazMovement_Constructor()
        {
            // Arrange
            var expectedBoostTimeLimit = 10f;
            _mockLazMovement.BoostTimeLimit = expectedBoostTimeLimit;
            
            //Act
            var lazMovement = new LazMovement(_mockLazMovement, _lazo);
            
            //Assert
            Assert.AreEqual(expectedBoostTimeLimit, lazMovement.BoostTimeLimit, "Make sure the constructor is passing in the correct data");
        }

        [Test]
        public void Test_OnSpeedChange()
        {
            // Arrange
            var lazMovement = new LazMovement(_mockLazMovement, _lazo);
            var increment = 0;
            lazMovement.OnSpeedChanges += speed =>
            {
                increment++;
            };
            
            // Act
            lazMovement.SetSpeedComponent();
            
            // Assert
            Assert.AreEqual(1, increment, "On Speed changed should have been called 1 time");
        }

        [Test]
        public void Test_DirectionFacingUp()
        {
            // Arrange
            var inputDirectionUp = Vector2.up;
            var up = new Vector3(0, 0, 1);
            _mockLazMovement.CurvatureRate = 1f;
            var lazMovement = new LazMovement(_mockLazMovement, _lazo);
            
            // Act
            lazMovement.OnMovementPressed(inputDirectionUp);
            lazMovement.UpdateLazTurning(up);
            
            //Assert
            Assert.AreEqual(up, lazMovement.GetCurrentDirection, "Direction should be facing up");
        }
        
        [Test]
        public void Test_DirectionFacingLeft()
        {
            // Arrange
            var inputDirectionLeft = Vector2.left;
            var left = new Vector3(-1, 0, 0);
            _mockLazMovement.CurvatureRate = 1f;
            var lazMovement = new LazMovement(_mockLazMovement, _lazo);
            
            // Act
            lazMovement.OnMovementPressed(inputDirectionLeft);
            lazMovement.UpdateLazTurning(left);
            
            //Assert
            Assert.AreEqual(left, lazMovement.GetCurrentDirection, "Direction should be facing left");
        }
        
        [Test]
        public void Test_DirectionFacingDown()
        {
            // Arrange
            var inputDirectionDown = Vector2.down;
            var down = new Vector3(0, 0, -1);
            _mockLazMovement.CurvatureRate = 1f;
            var lazMovement = new LazMovement(_mockLazMovement, _lazo);
            
            // Act
            lazMovement.OnMovementPressed(inputDirectionDown);
            lazMovement.UpdateLazTurning(down);
            
            //Assert
            Assert.AreEqual(down, lazMovement.GetCurrentDirection, "Direction should be facing left");
        }
        
        [Test]
        public void Test_DirectionFacingRight()
        {
            // Arrange
            var inputDirectionRight = Vector2.right;
            var right = new Vector3(1, 0, 0);
            _mockLazMovement.CurvatureRate = 1f;
            var lazMovement = new LazMovement(_mockLazMovement, _lazo);
            
            // Act
            lazMovement.OnMovementPressed(inputDirectionRight);
            lazMovement.UpdateLazTurning(right);
            
            //Assert
            Assert.AreEqual(right, lazMovement.GetCurrentDirection, "Direction should be facing left");
        }
        
        [Test]
        public void Test_GetVelocity_AccelerationOf30_Upwards()
        {
            // Arrange
            var up = new Vector3(0, 0, 1);
            var expectedSpeed = 30;
            _mockLazMovement.CurvatureRate = 1f;
            _mockLazMovement.Acceleration = expectedSpeed;
            _mockLazMovement.BaseMaxSpeed = 100f;
            var lazMovement = new LazMovement(_mockLazMovement, _lazo);
            lazMovement.OnMovementPressed(new Vector2(0, 1));
            lazMovement.UpdateLazTurning(up);
            
            // Act
            lazMovement.SetSpeedComponent();
            
            //Assert
            Assert.AreEqual(new Vector3(0, 0, expectedSpeed), lazMovement.GetVelocity, "Laz should be moving UP, at speed of 30");
        }
        
        [Test]
        public void Test_GetVelocity_Upwards_MaxSpeedOf20()
        {
            // Arrange
            var up = new Vector3(0, 0, 1);
            var expectedMaxSpeed = 20;
            _mockLazMovement.CurvatureRate = 1f;
            _mockLazMovement.Acceleration = 100f;
            _mockLazMovement.BaseMaxSpeed = expectedMaxSpeed;
            var lazMovement = new LazMovement(_mockLazMovement, _lazo);
            lazMovement.OnMovementPressed(new Vector2(0, 1));
            lazMovement.UpdateLazTurning(up);
            
            // Act
            lazMovement.SetSpeedComponent();
            
            //Assert
            Assert.AreEqual(new Vector3(0, 0, expectedMaxSpeed), lazMovement.GetVelocity, "Laz should be moving UP, at speed of 20 Max Speed");
        }
        
        [Test]
        public void Test_GetVelocity_Upwards_WithLazoSpeed_MaxLazoSpeedOf90()
        {
            // Arrange
            var up = new Vector3(0, 0, 1);
            var expectedLazoSpeed = 90;
            _mockLazMovement.CurvatureRate = 1f;
            _mockLazMovement.Acceleration = 100f;
            _mockLazMovement.BaseMaxSpeed = 20;
            _mockLazMovement.LazoMaxSpeed = expectedLazoSpeed;
            _lazo.SetLazoActive(true);
            var lazMovement = new LazMovement(_mockLazMovement, _lazo);
            lazMovement.OnMovementPressed(new Vector2(0, 1));
            lazMovement.UpdateLazTurning(up);
            
            // Act
            lazMovement.DeactivateBoost();
            lazMovement.SetSpeedComponent();
            
            //Assert
            Assert.AreEqual(new Vector3(0, 0, expectedLazoSpeed), lazMovement.GetVelocity, "Laz should be Lazoing, and at lazo speed of 90");
        }
        
        [Test]
        public void Test_GetVelocity_Upwards_TurnOffLazo_MaxBaseSpeedOf20()
        {
            // Arrange
            var up = new Vector3(0, 0, 1);
            var expectedBaseMaxSpeed = 20;
            _mockLazMovement.CurvatureRate = 1f;
            _mockLazMovement.Acceleration = 100f;
            _mockLazMovement.BaseMaxSpeed = expectedBaseMaxSpeed;
            _mockLazMovement.LazoMaxSpeed = 90;
            _lazo.SetLazoActive(false);
            var lazMovement = new LazMovement(_mockLazMovement, _lazo);
            lazMovement.OnMovementPressed(new Vector2(0, 1));
            lazMovement.UpdateLazTurning(up);
            
            // Act
            lazMovement.DeactivateBoost();
            lazMovement.SetSpeedComponent();
            
            //Assert
            Assert.AreEqual(new Vector3(0, 0, expectedBaseMaxSpeed), lazMovement.GetVelocity, "Laz should NOT be Lazoing, and at base speed of 20");
        }
        
        [Test]
        public void Test_GetVelocity_Reset()
        {
            // Arrange
            var up = new Vector3(0, 0, 1);
            var expectedBaseSpeed = 20;
            _mockLazMovement.CurvatureRate = 1f;
            _mockLazMovement.Acceleration = 100f;
            _mockLazMovement.BaseMaxSpeed = expectedBaseSpeed;
            _mockLazMovement.LazoMaxSpeed = 50f;
            _lazo.SetLazoActive(true);
            var lazMovement = new LazMovement(_mockLazMovement, _lazo);
            lazMovement.OnMovementPressed(new Vector2(0, 1));
            lazMovement.UpdateLazTurning(up);
            lazMovement.DeactivateBoost();
            lazMovement.SetSpeedComponent();
            
            // Act
            lazMovement.Reset();
            lazMovement.SetSpeedComponent();
            
            //Assert
            Assert.AreEqual(new Vector3(0, 0, expectedBaseSpeed), lazMovement.GetVelocity, "Laz Reset should have base max speed");
        }
        
        [Test]
        public void Test_GetVelocity_CleanUp()
        {
            // Arrange
            var up = new Vector3(0, 0, 1);
            var expectedBaseSpeed = 20;
            _mockLazMovement.CurvatureRate = 1f;
            _mockLazMovement.Acceleration = 100f;
            _mockLazMovement.BaseMaxSpeed = expectedBaseSpeed;
            _mockLazMovement.LazoMaxSpeed = 50f;
            _lazo.SetLazoActive(true);
            var lazMovement = new LazMovement(_mockLazMovement, _lazo);
            lazMovement.OnMovementPressed(new Vector2(0, 1));
            lazMovement.UpdateLazTurning(up);
            lazMovement.DeactivateBoost();
            lazMovement.SetSpeedComponent();
            
            // Act
            lazMovement.CleanUp();
            
            //Assert
            Assert.AreEqual(Vector3.zero, lazMovement.GetVelocity, "Clean Up should make Velocity 0 out");
            Assert.AreEqual(Vector3.zero, lazMovement.GetCurrentDirection, "Clean Up should Current Direction 0");
        }
        
        [Test]
        public void Test_GetVelocity_DeccelerationOf30_Upwards()
        {
            // Arrange
            var up = new Vector3(0, 0, 1);
            _mockLazMovement.CurvatureRate = 1f;
            _mockLazMovement.Acceleration = 100f;
            _mockLazMovement.Deceleration = 100f;
            _mockLazMovement.BaseMaxSpeed = 10;
            var lazMovement = new LazMovement(_mockLazMovement, _lazo);
 
            
            // Act
            lazMovement.OnMovementCancelled();
            lazMovement.UpdateLazTurning(up);
            lazMovement.SetSpeedComponent();
            
            //Assert
            Assert.AreEqual(Vector3.zero, lazMovement.GetVelocity, "Laz should be moving UP, at speed of 30");
        }

        [Test]
        public void Test_Fail()
        {
            Assert.IsFalse(false);
        }
    } 
}

