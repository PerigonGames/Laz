using Laz;
using NUnit.Framework;

namespace Tests
{
    public class LazoTests
    {
        private MockLazoProperties _mockLazoProperties;
        
        [SetUp]
        public void Setup()
        {
            _mockLazoProperties = new MockLazoProperties();
        }
        
        [Test]
        public void Test_Lazo_Constructor()
        {
            // Arrange
            _mockLazoProperties.TimeToLivePerPoint = 10f;
            _mockLazoProperties.CoolDown = 4f;
            
            // Act
            var lazo = new Lazo(_mockLazoProperties, new ILazoWrapped[]  {}, new MockBoost());
            
            // Assert
            Assert.IsFalse(lazo.IsLazoing, "Is lazoing should be false at init");
            Assert.AreEqual(10, lazo.TimeToLivePerPoint, "Time to live should be 10");
            Assert.AreEqual(4, lazo.CoolDown, "Cooldown should be 4");
        }
        
        [Test]
        public void Test_CleanUp()
        {
            // Arrange
            _mockLazoProperties.TimeToLivePerPoint = 10f;
            _mockLazoProperties.CoolDown = 4f;
            var lazo = new Lazo(_mockLazoProperties, new ILazoWrapped[]  {}, new MockBoost());
            lazo.SetLazoActive(true);
            
            // Act
            lazo.CleanUp();
            
            // Assert
            Assert.IsFalse(lazo.IsLazoing, "Is lazoing should be false at init");
        }
        
        [Test]
        public void Test_Reset()
        {
            // Arrange
            _mockLazoProperties.TimeToLivePerPoint = 10f;
            _mockLazoProperties.CoolDown = 4f;
            var lazo = new Lazo(_mockLazoProperties, new ILazoWrapped[]  {}, new MockBoost());
            lazo.SetLazoActive(true);
            int called = 0;
            lazo.OnLazoLimitChanged += limitPercentage =>
            {
                called++;
            };
            
            // Act
            lazo.Reset();
            
            // Assert
            Assert.AreEqual(1, called, "Travelled Distance should be set 1 time");
            Assert.IsFalse(lazo.IsLazoing, "Is lazoing should be false at init");
        }
        
        [Test]
        public void Test_ResetTravelledDistance()
        {
            // Arrange
            _mockLazoProperties.DistanceLimitOfLazo = 1;
            
            var lazo = new Lazo(_mockLazoProperties, new ILazoWrapped[]  {}, new MockBoost());
            lazo.SetLazoActive(true);
            int called = 0;
            lazo.OnLazoLimitChanged += limitPercentage =>
            {
                called++;
            };
            
            // Act
            lazo.Reset();
            
            // Assert
            Assert.AreEqual(1, called, "Travelled Distance should be set 1 time");
            Assert.IsFalse(lazo.IsLazoing, "Is lazoing should be false at init");
        }
    }
}