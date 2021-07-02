using System;
using Laz;

namespace Tests
{
    public class MockStateManager : IStateManager
    {
        public event Action<State> OnStateChanged;
        private State _currentState = State.Play;

        public State GetState()
        {
            return _currentState;
        }

        public void SetState(State state)
        {
            _currentState = state;
        }
    }
}