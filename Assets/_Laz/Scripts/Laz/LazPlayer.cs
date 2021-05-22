using UnityEngine;

namespace Laz
{
    public class LazPlayer
    {
        private LazMovement _lazMovement;
        private Lazo _lazo;
        private Vector3 _spawnPosition = Vector3.zero;

        public LazMovement Movement => _lazMovement;
        public Lazo LazoTool => _lazo;

        public Vector3 SpawnPosition => _spawnPosition;

        public void SetMovement(ILazMovementProperty movementProperty, Lazo lazo)
        {
            _lazMovement = new LazMovement(movementProperty, lazo);
        }

        public void SetLazo(ILazoProperties lazoProperties, ILazoWrapped[] objectOfInterest, IBoost boost)
        {
            _lazo = new Lazo(lazoProperties, objectOfInterest, boost);
        }

        public void SetSpawn(Vector3 spawnPosition)
        {
            _spawnPosition = spawnPosition;
        }
    }
}