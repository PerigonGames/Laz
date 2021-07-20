using Sirenix.OdinInspector;
using UnityEngine;

namespace Laz
{
    public interface IChomperProperties : IEnemyProperties
    {
        float IdleRadius { get; }
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

        public float IdleRadius => _idleRadius;

    }
}

