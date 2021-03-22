using UnityEngine;

namespace Laz
{
    public interface ILazoProperties
    {
        float TimeToLivePerPoint { get; }
        float RateOfRecordingPosition { get; }
    }

    [CreateAssetMenu(fileName = "Lazo Properties", menuName = "Laz/Lazo Properties", order = 1)]
    public class LazoPropertiesScriptableObject : ScriptableObject, ILazoProperties
    {
        [SerializeField]
        private float _timeToLivePerPoint = 5f;
        [SerializeField]
        private float _rateOfRecordingPosition = 0.5f;

        public float TimeToLivePerPoint => _timeToLivePerPoint;
        public float RateOfRecordingPosition => _rateOfRecordingPosition;
    }
}