using Sirenix.OdinInspector;
using UnityEngine;

namespace Laz
{
    public interface IEnemyProperties
    {
        float IdleSpeed { get; }
    }
    
    public class EnemyPropertiesScriptableObject : ScriptableObject
    {
        [Title("Generic Enemy Properties")]
        [SerializeField] 
        private float _idleSpeed = 5f;
        
        public float IdleSpeed => _idleSpeed;
    }
}