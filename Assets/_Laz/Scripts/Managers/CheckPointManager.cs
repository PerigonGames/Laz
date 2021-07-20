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
            if (_checkpoints.IsNullOrEmpty())
            {
                Debug.LogError("No Checkpoints Found!");
                return;
            }

            _laz = laz;
            
            for (int i = 0; i < _checkpoints.Length; i++)
            {
                Checkpoint checkpoint = _checkpoints[i];

                if (checkpoint == null)
                {
                    Debug.LogError("Null Checkpoint");
                    continue;
                }
                
                checkpoint.OnCheckpointActivation += SetNewCheckpoint;

                if (i == 0)
                {
                    _activeCheckpoint = checkpoint;
                    _activeCheckpoint.IsActiveCheckpoint = true;
                    _initialCheckPointPosition = _activeCheckpoint.transform.position;
                    _laz.SetSpawn(_initialCheckPointPosition);
                }
            }
        }
        
        public Vector3 GetActiveCheckpointPosition()
        {
            return _activeCheckpoint != null ? _activeCheckpoint.transform.position : _initialCheckPointPosition;
        }
        
        public void Reset()
        {
            _laz.SetSpawn(_activeCheckpoint != null ? _activeCheckpoint.transform.position : _initialCheckPointPosition);
        }

        private void SetNewCheckpoint(Checkpoint checkpoint)
        {
            _activeCheckpoint.IsActiveCheckpoint = false;
            _activeCheckpoint = checkpoint;
            _activeCheckpoint.IsActiveCheckpoint = true;
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
