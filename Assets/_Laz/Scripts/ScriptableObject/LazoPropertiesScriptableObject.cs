using UnityEngine;

namespace Laz
{
    public interface ILazoProperties
    {
        float TimeToLivePerPoint { get; }
        float RateOfRecordingPosition { get; }
        float DistanceLimitOfLazo { get; }
        float CoolDown { get; }
    }

    [CreateAssetMenu(fileName = "Lazo Properties", menuName = "Laz/Lazo Properties", order = 1)]
    public class LazoPropertiesScriptableObject : ScriptableObject, ILazoProperties
    {
        [SerializeField]
        private float _timeToLivePerPoint = 5f;
        [SerializeField]
        private float _rateOfRecordingPosition = 0.5f;
        [SerializeField] 
        private float _distanceLimitOfLazo = 5f;
        [SerializeField] 
        private float _coolDown = 2f;

        public float TimeToLivePerPoint => _timeToLivePerPoint;
        public float RateOfRecordingPosition => _rateOfRecordingPosition;
        public float DistanceLimitOfLazo => _distanceLimitOfLazo;
        public float CoolDown => _coolDown;
    }
}