using UnityEngine;

namespace Laz
{
    public interface ILazMovementProperty
    {
        float Acceleration { get; }
        float Deceleration { get; }
        float BaseMaxSpeed { get; }
        float BoostSpeed { get; }
        float CurvatureRate { get; }
        float BoostTimeLimit { get; }
    }
    [CreateAssetMenu(fileName = "Laz Movement", menuName = "Laz/Movement", order = 1)]
    public class LazMovementPropertyScriptableObject : ScriptableObject, ILazMovementProperty
    {
        [SerializeField]
        private float _acceleration = 1f;
        [SerializeField]
        private float _deceleration = 1f;
        [SerializeField]
        private float _baseMaxSpeed = 5f;
        [SerializeField]
        [Range(0.005f, 0.1f)]
        private float _curvatureRate = 0.1f;

        [Header("Boost Properties")] 
        [SerializeField]
        private float _boostTimeLimit = 2f;
        [SerializeField]
        private float _boostSpeed = 10f;
        
        public float Acceleration => _acceleration;
        public float Deceleration => _deceleration;
        public float BaseMaxSpeed => _baseMaxSpeed;
        public float BoostSpeed => _boostSpeed;
        public float CurvatureRate => _curvatureRate;
        public float BoostTimeLimit => _boostTimeLimit;
    }
}