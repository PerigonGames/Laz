using System.Collections.Generic;
using PerigonGames;
using UnityEngine;

namespace Laz
{
    public class LazoWallObjectPooler : ObjectPool<LazoWallBehaviour>
    {
        private const int SizeToSpawn = 300;
        
        public static readonly int Key = 651;
        
        [SerializeField] private LazoWallBehaviour _lazoWallPrefab = null;

        public void Initialize()
        {
            var pooledObjects = new List<PoolingObject<LazoWallBehaviour>>();
            pooledObjects.Add(CreatePoolingObject(Key, _lazoWallPrefab, SizeToSpawn));
            base.Initialize(pooledObjects);
        }
    }
}