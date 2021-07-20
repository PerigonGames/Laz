using PerigonGames;
using UnityEngine;

namespace Laz
{
    public class CheckPointManager : MonoBehaviour
    {
        [SerializeField] private Checkpoint[] _checkpoints;
        private Checkpoint _activeCheckpoint = null;
        private Vector3 _initialCheckPointPosition = Vector3.zero;

        public void Initialize()
        {
            if (_checkpoints.IsNullOrEmpty())
            {
                return;
            }
            
            for (int i = 0; i < _checkpoints.Length; i++)
            {
                Checkpoint checkpoint = _checkpoints[i];

                if (checkpoint == null)
                {
                    continue;
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
        
        public Vector3 GetActiveCheckpointPosition()
        {
            return _activeCheckpoint != null ? _activeCheckpoint.transform.position : _initialCheckPointPosition;
        }
        
        //Not sure how this will be implemented properly with current setup
        public void Reset()
        {
            if (_checkpoints.IsNullOrEmpty())
            {
                return;
            }

            foreach (Checkpoint checkpoint in _checkpoints)
            {
                if (checkpoint != null)
                {
                    checkpoint.IsActiveCheckpoint = false;
                }
            }
        }

        private void SetNewCheckpoint(Checkpoint checkpoint)
        {
            if (_activeCheckpoint != null)
            {
                _activeCheckpoint.IsActiveCheckpoint = false;
            }

            _activeCheckpoint = checkpoint;
            _activeCheckpoint.IsActiveCheckpoint = true;
        }

        private void OnDestroy()
        {
            if (_checkpoints == null)
            {
                return;
            }

            foreach (Checkpoint checkpoint in _checkpoints)
            {
                checkpoint.OnCheckpointActivation -= SetNewCheckpoint;
            }
        }
    }
}
