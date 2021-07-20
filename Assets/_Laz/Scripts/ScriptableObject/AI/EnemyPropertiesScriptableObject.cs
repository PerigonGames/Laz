using Sirenix.OdinInspector;
using UnityEngine;

namespace Laz
{
    public interface IEnemyProperties
    {
        float Speed { get; }
    }
    
    public class EnemyPropertiesScriptableObject : ScriptableObject
    {
        [Title("Generic Enemy Properties")]
        [SerializeField] 
        private float _speed = 5f;
        
        public float Speed => _speed;
    }
}