using Sirenix.OdinInspector;
using UnityEngine;

namespace Laz
{
    public interface ILazMovementProperty
    {
        float Acceleration { get; }
        float Deceleration { get; }
        float BaseMaxSpeed { get; }
        float BoostSpeed { get; }
        float LazoMaxSpeed { get; }
        float CurvatureRate { get; }
        float BoostTimeLimit { get; }
    }
    [CreateAssetMenu(fileName = "Laz Movement", menuName = "Laz/Player/Movement", order = 1)]
    [InlineEditor]
    public class LazMovementPropertyScriptableObject : ScriptableObject, ILazMovementProperty
    {
        // [SerializeField]
        // private float _acceleration = 1f;
        // [SerializeField]
        // private float _deceleration = 1f;
        // [SerializeField]
        // private float _baseMaxSpeed = 5f;
        // [SerializeField]
        // [Range(0.005f, 0.1f)]
        // private float _curvatureRate = 0.1f;
        // [SerializeField]
        // private float _lazMaxSpeed = 10f;
        //
        // [Header("Boost Properties")] 
        // [SerializeField]
        // private float _boostTimeLimit = 2f;
        // [SerializeField]
        // private float _boostSpeed = 20f;

        public float acceleration = 1f;
        public float deceleration = 1f;
        public float baseMaxSpeed = 5f;
        [Range(0.005f, 0.1f)] public float curvatureRate = 0.1f;
        public float lazMaxSpeed = 10f;

        [Header("Boost Properties")] 
        public float boostTimeLimit = 2f;
        public float boostSpeed = 20f;
        
        public float Acceleration => acceleration;
        public float Deceleration => deceleration;
        public float BaseMaxSpeed => baseMaxSpeed;
        public float BoostSpeed => boostSpeed;
        public float CurvatureRate => curvatureRate;
        public float BoostTimeLimit => boostTimeLimit;
        public float LazoMaxSpeed => lazMaxSpeed;
    }
}