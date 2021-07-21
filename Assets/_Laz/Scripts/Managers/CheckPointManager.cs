using System;
using PerigonGames;
using UnityEngine;

namespace Laz
{
    public class CheckPointManager : MonoBehaviour
    {
        [SerializeField] private Checkpoint[] _checkpoints;
        private Checkpoint _activeCheckpoint = null;
        private Vector3 _initialCheckPointPosition = Vector3.zero;
        private LazPlayer _laz = null;

        public void Initialize(LazPlayer laz)
        {
            _laz = laz;
            _laz.SetSpawn(_initialCheckPointPosition);
        }

        private void Awake()
        {
            if (_checkpoints.IsNullOrEmpty())
            {
                Debug.LogError("No Checkpoints found in the CheckPointManager!");
            }
            
            for (int i = 0; i < _checkpoints.Length; i++)
            {
                Checkpoint checkpoint = _checkpoints[i];

                if (checkpoint == null)
                {
                    Debug.LogError("There are Null Checkpoints within the CheckpointManager");
                }
                
                checkpoint.OnCheckpointActivation += SetNewCheckpoint;

                if (i == 0)
                {
                    _activeCheckpoint = checkpoint;
                    _activeCheckpoint.IsActiveCheckpoint = true;
                    _initialCheckPointPosition = _activeCheckpoint.transform.position;
                }
            }
        }

        private void SetNewCheckpoint(Checkpoint checkpoint)
        {
            _activeCheckpoint.IsActiveCheckpoint = false;
            _activeCheckpoint = checkpoint;
            _activeCheckpoint.IsActiveCheckpoint = true;
            _laz.SetSpawn(_activeCheckpoint.transform.position);
        }

        private void OnDestroy()
        {
            foreach (Checkpoint checkpoint in _checkpoints)
            {
                checkpoint.OnCheckpointActivation -= SetNewCheckpoint;
            }
        }
    }
}
