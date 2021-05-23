using Laz;

namespace Tests
{
    public class MockBoost : IBoost
    {
        public bool IsBoostActivated { get; }
        
        public void SetBoostActive(bool activate)
        {
            // Stub
        }
    }
}

