using Sirenix.OdinInspector;
using UnityEngine;

namespace Laz
{
    public interface IChomperProperties : IEnemyProperties
    {
        float IdleRadius { get; }
        float AgroSpeed { get; }
        float AgroDetectionRadius { get; }
        float ExtraDistanceToTravel { get; }
       
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
        [InfoBox("Agro Speed - Speed at which Chomper moves when agro")]
        [SerializeField] 
        private float _agroSpeed = 0;
        [InfoBox("Detection Radius - Starts Agro when Lazo within detection radius")]
        [SerializeField] 
        private float _agroDetectionRadius = 0;
        [InfoBox("Extra distance to travel after completing the Lazo trail")]
        [SerializeField] 
        private float _extraDistanceToTravel = 0;

        
        public float IdleRadius => _idleRadius;
        public float AgroSpeed => _agroSpeed;
        public float AgroDetectionRadius => _agroDetectionRadius;
        public float ExtraDistanceToTravel => _extraDistanceToTravel;
        
    }
}

