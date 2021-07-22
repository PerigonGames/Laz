using Sirenix.OdinInspector;
using UnityEngine;

namespace Laz
{
    public interface IChomperProperties : IEnemyProperties
    {
        float IdleRadius { get; }
        float AgroDetectionRadius { get; }
    }
    
    [InlineEditor]
    [CreateAssetMenu(fileName = "Chomper", menuName = "Laz/AI/Chomper", order = 1)]
    public class ChomperPropertiesScriptableObject : EnemyPropertiesScriptableObject, IChomperProperties
    {
        [Title("Chomper Properties")]
        [DetailedInfoBox("Idle area allowed to move",
            "Circular Area the chomper is allowed to idle around from spawn")]
        [SerializeField]
        private float _idleRadius = 0;
        [InfoBox("Detection Radius - Starts Agro when Lazo within detection radius")]
        [SerializeField] 
        private float _agroDetectionRadius = 0;

        public float IdleRadius => _idleRadius;

        public float AgroDetectionRadius => _agroDetectionRadius;

    }
}

