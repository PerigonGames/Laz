using System;
using UnityEngine;
using UnityEngine.UI;

namespace Laz
{
    public class EndGamePanelBehaviour : MonoBehaviour
    {
        [SerializeField] private Button _reset = null;

        private void Awake()
        {
            _reset.onClick.AddListener(Reset);
            StateManager.Instance.OnStateChanged += HandleOnStateChanged;
        }

        private void OnDestroy()
        {
            StateManager.Instance.OnStateChanged -= HandleOnStateChanged;
        }

        private void HandleOnStateChanged(State state)
        {
            if (state == State.WinGame)
            {
                transform.localScale = Vector3.one;
            }
        }

        private void Reset()
        {
            StateManager.Instance.SetState(State.PreGame);
            transform.localScale = Vector3.zero;
        }
    }
}
