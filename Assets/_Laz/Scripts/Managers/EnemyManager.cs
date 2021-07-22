using UnityEngine;

namespace Laz
{
    public class EnemyManager : MonoBehaviour
    {
        private EnemyBehaviour[] _enemies = null;
        
        public void Initialize(Lazo lazo)
        {
            InitializeEnemies(lazo);
        }

        public void Reset()
        {
            foreach (var enemy in _enemies)
            {
                if (enemy is ChomperBehaviour chomper)
                {
                    chomper.Reset();
                }
            }
        }

        public void CleanUp()
        {
            foreach (var enemy in _enemies)
            {
                if (enemy is ChomperBehaviour chomper)
                {
                    chomper.CleanUp();
                }
            }
        }
        
        private void InitializeEnemies(Lazo lazo)
        {
            foreach (var enemy in _enemies)
            {
                if (enemy is ChomperBehaviour chomper)
                {
                    chomper.Initialize(lazo);
                }
            }
        }
        
        #region Mono
        
        private void Awake()
        { 
            _enemies = FindObjectsOfType<EnemyBehaviour>();
        }
        
        #endregion

    }
}

