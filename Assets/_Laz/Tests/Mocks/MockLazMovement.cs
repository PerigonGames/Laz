using Laz;

namespace Tests
{
    public class MockLazMovement : ILazMovementProperty
    {
        public MockLazMovement(
            float acceleration = 5f,
            float deceleration = 5f,
            float baseMaxSpeed = 5f,
            float boostSpeed = 20f,
            float lazoMaxSpeed = 15f,
            float curvatureRate = 5f,
            float boostTimeLimit = 2f)
        {
            Acceleration = acceleration;
            Deceleration = deceleration;
            BaseMaxSpeed = baseMaxSpeed;
            BoostSpeed = boostSpeed;
            LazoMaxSpeed = lazoMaxSpeed;
            CurvatureRate = curvatureRate;
            BoostTimeLimit = boostTimeLimit;
        }

        public float Acceleration { get; set; }
        public float Deceleration { get; set;}
        public float BaseMaxSpeed { get; set;}
        public float BoostSpeed { get; set; }
        public float LazoMaxSpeed { get; set; }
        public float CurvatureRate { get; set; }
        public float BoostTimeLimit { get; set; }
    }
}

