using System;
using PerigonGames;
using UnityEngine;

namespace Laz
{
    public class RandomPositionGenerator
    {
        private readonly Vector3 _spawnPosition;
        private readonly float _radius;
        
        private readonly IRandomUtility _randomUtility;
        
        public RandomPositionGenerator(Vector3 spawnPosition, float radius, IRandomUtility randomUtility = null)
        {
            _randomUtility = randomUtility ?? new RandomUtility();
            _spawnPosition = spawnPosition;
            _radius = radius;
        }

        public Vector3 GetRandomPosition()
        {
            var angle = RandomAngle();
            var randomRadius = RandomRadius();
            var x = Math.Cos(angle) * randomRadius;
            var z = Math.Sin(angle) * randomRadius;
            return _spawnPosition + new Vector3((float) x, 0, (float) z);
        }

        private double RandomAngle()
        {
            return _randomUtility.NextDouble() * Math.PI * 2;
        }
        
        private double RandomRadius()
        {
            return _randomUtility.NextDouble() * _radius;
        }

    }
}