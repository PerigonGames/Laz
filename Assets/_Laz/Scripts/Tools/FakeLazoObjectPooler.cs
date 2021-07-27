using System.Collections.Generic;
using PerigonGames;
using UnityEngine;

namespace Laz
{
    public class FakeLazoObjectPooler : ObjectPool<FakeLazoBehaviour>
    {
        private const int SIZE_TO_SPAWN = 2;

        public static int Key => 123;

        [SerializeField] private FakeLazoBehaviour _fakeLazoBehaviour = null;

        public void Initialize()
        {
            var pooledObjects = new List<PoolingObject<FakeLazoBehaviour>>();
            pooledObjects.Add(CreatePoolingObject(Key, _fakeLazoBehaviour, SIZE_TO_SPAWN));
            base.Initialize(pooledObjects);
        }
    }
}

