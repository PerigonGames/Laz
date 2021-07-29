using Sirenix.OdinInspector;
using UnityEngine;

namespace Laz
{
    public interface ILazoColorProperties
    {
        Gradient NormalGradient { get; }
        Gradient FrozenColor { get; }
    }
    [InlineEditor]
    [CreateAssetMenu(fileName = "Lazo Color Gradient", menuName = "Laz/Player/Lazo Colors", order = 1)]
    public class LazoColorPropertiesScriptableObject : ScriptableObject, ILazoColorProperties
    {
        [SerializeField] private Gradient _normalColor = null;
        [SerializeField] private Gradient _frozenColor = null;

        public Gradient NormalGradient => _normalColor;
        public Gradient FrozenColor => _frozenColor;
    }
}

