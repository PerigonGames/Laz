using System.Collections.Generic;
using PerigonGames;
using UnityEngine;

namespace Laz
{
    public class ParticleEffectsObjectPooler : ObjectPool<ParticleSystem>
    {
        [SerializeField] private ParticleSystem _planetoidExplosion = null;
            
        public void Initialize(int amountToInstantiate)
        {
            var pooledObjects = new List<PoolingObject<ParticleSystem>>();
            pooledObjects.Add(CreatePoolingObject(ParticleSystemObjectPoolTag.PlanetoidExplosion, _planetoidExplosion, amountToInstantiate));
            base.Initialize(pooledObjects);
        }
    }

    public struct ParticleSystemObjectPoolTag
    {
        public static int PlanetoidExplosion = 123;
    }
}

