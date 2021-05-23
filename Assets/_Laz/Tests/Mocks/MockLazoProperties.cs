using Laz;

namespace Tests
{
    public class MockLazoProperties : ILazoProperties
    {
        public MockLazoProperties(
            float timeToLivePerPoint = 5f, 
            float rateOfRecordingPosition = 0.5f, 
            float distanceLimitOfLazo = 50f, 
            float coolDown = 0.5f)
        {
            TimeToLivePerPoint = timeToLivePerPoint;
            RateOfRecordingPosition = rateOfRecordingPosition;
            DistanceLimitOfLazo = distanceLimitOfLazo;
            CoolDown = coolDown;
        }

        public float TimeToLivePerPoint { get; set; }
        public float RateOfRecordingPosition { get; set; }
        public float DistanceLimitOfLazo { get; set; }
        public float CoolDown { get; set; }
    }
}

