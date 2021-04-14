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

        public void Set(ILazMovementProperty movementProperty)
        {
            _lazMovement = new LazMovement(movementProperty);
        }

        public void Set(ILazoProperties lazoProperties, IObjectOfInterest[] objectOfInterest)
        {
            _lazo = new Lazo(lazoProperties, objectOfInterest);
        }

        public void SetSpawn(Vector3 spawnPosition)
        {
            _spawnPosition = spawnPosition;
        }
    }
}