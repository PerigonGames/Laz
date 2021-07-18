using UnityEngine;

namespace Laz
{
    public class EnemyManager : MonoBehaviour
    {
        private EnemyBehaviour[] _enemies = null;
        
        public void Initialize()
        {
            InitializeEnemies();
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
        
        private void InitializeEnemies()
        {
            foreach (var enemy in _enemies)
            {
                if (enemy is ChomperBehaviour chomper)
                {
                    chomper.Initialize();
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

