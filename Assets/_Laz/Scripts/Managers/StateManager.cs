using System;
using UnityEngine;

namespace Laz
{
    public enum State
    {
        Pause,
        Play,
        Death,
        WinGame,
        EndGame
    }

    public interface IStateManager
    {
        event Action<State> OnStateChanged;
        State GetState();

        void SetState(State state);
    }

    /// <summary>
    /// The 'Singleton' class
    /// </summary>
    public class StateManager: IStateManager
    {
        private static readonly StateManager _instance = new StateManager();

        private State _gameState = State.Play;
        public event Action<State> OnStateChanged;

        public static StateManager Instance => _instance;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static StateManager()
        {
        }

        private StateManager()
        {
        }


        /// <summary>
        /// Sets the state of the application
        /// </summary>
        /// <returns></returns>
        public State GetState()
        {
            return _gameState;
        }

        public void SetState(State state)
        {
            _gameState = state;
            OnStateChanged?.Invoke(state);
        }
    }
}