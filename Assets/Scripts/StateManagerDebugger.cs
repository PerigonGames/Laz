using UnityEngine;

namespace Laz
{
    public class StateManagerDebugger : MonoBehaviour
    {
        private StateManager _stateManager;
        private void Awake()
        {
            _stateManager = StateManager.Instance;
        }

        [ContextMenu("Set Death")]
        public void SetDeath()
        {
            _stateManager.SetState(State.Death);
        }

        [ContextMenu("Set Alive")]
        public void SetAlive()
        {
            _stateManager.SetState(State.Play);
        }
    }
}