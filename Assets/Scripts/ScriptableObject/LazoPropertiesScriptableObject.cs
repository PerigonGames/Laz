using UnityEngine;

namespace Laz
{
    public interface ILazoProperties
    {
        float LifeTimePerPoint { get; }
        float RateOfRecordingPosition { get; }
    }

    [CreateAssetMenu(fileName = "Lazo Properties", menuName = "Laz/Laz Properties", order = 1)]
    public class LazoPropertiesScriptableObject : ScriptableObject, ILazoProperties
    {
        [SerializeField]
        private float _lifeTimePerPoint = 5f;
        [SerializeField]
        private float _rateOfRecordingPosition = 0.5f;

        public float LifeTimePerPoint => _lifeTimePerPoint;
        public float RateOfRecordingPosition => _rateOfRecordingPosition;
    }
}