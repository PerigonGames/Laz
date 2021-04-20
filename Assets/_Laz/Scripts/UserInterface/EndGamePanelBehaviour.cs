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
        }

        private void Reset()
        {
            
        }
    }
}
