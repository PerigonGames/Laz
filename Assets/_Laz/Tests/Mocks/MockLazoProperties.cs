using Laz;
using UnityEngine;

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

        public ILazoColorProperties LazoColorProperties => new MockColorProperties();
    }

    public class MockColorProperties : ILazoColorProperties
    {
        public Gradient NormalGradient => new Gradient();
        public Gradient FrozenColor => new Gradient();
        public ILazoSplatter NormalSplatter => new MockNormalLazoSplatter();
        public ILazoSplatter FrozenSplatter => new MockFrozenLazoSplatter();
    }

    public class MockNormalLazoSplatter : ILazoSplatter
    {
        public Color MinColor => Color.blue;
        public Color MaxColor => Color.cyan;
    }
    
    public class MockFrozenLazoSplatter : ILazoSplatter
    {
        public Color MinColor => Color.yellow;
        public Color MaxColor => Color.red;
    }
}

