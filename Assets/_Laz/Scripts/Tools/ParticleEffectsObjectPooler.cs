using System.Collections.Generic;
using PerigonGames;
using UnityEngine;

namespace Laz
{
    public class ParticleEffectsObjectPooler : ObjectPool<ParticleSystem>
    {
        [SerializeField] private ParticleSystem _planetoidExplosion = null;
            
        public void Initialize(int planetoids)
        {
            var pooledObjects = new List<PoolingObject<ParticleSystem>>();
            pooledObjects.Add(GenerateListOfPooledObjected(_planetoidExplosion, planetoids));
            base.Initialize(pooledObjects);
        }

        private PoolingObject<ParticleSystem> GenerateListOfPooledObjected(ParticleSystem system, int number)
        {
            var tag = ParticleSystemObjectPoolTag.PlanetoidExplosion;
            return new PoolingObject<ParticleSystem>(tag, system, number);
        }
    }

    public struct ParticleSystemObjectPoolTag
    {
        public static int PlanetoidExplosion = 123;
    }
}

