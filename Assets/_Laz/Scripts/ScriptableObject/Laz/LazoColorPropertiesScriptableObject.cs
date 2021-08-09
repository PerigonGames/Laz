using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Laz
{
    public interface ILazoColorProperties
    {
        Gradient NormalGradient { get; }
        Gradient FrozenColor { get; }
        ILazoSplatter NormalSplatter { get; }
        ILazoSplatter FrozenSplatter { get; }
    }
    [InlineEditor]
    [CreateAssetMenu(fileName = "Lazo Color Gradient", menuName = "Laz/Player/Lazo Colors", order = 1)]
    public class LazoColorPropertiesScriptableObject : ScriptableObject, ILazoColorProperties
    {
        [SerializeField] private Gradient _normalColor = null;
        [SerializeField] private Gradient _frozenColor = null;
        [SerializeField] private LazoSplatter _normalSplatter;
        [SerializeField] private LazoSplatter _frozenSplatter;
        
        public Gradient NormalGradient => _normalColor;
        public Gradient FrozenColor => _frozenColor;
        public ILazoSplatter NormalSplatter => _normalSplatter;
        public ILazoSplatter FrozenSplatter => _frozenSplatter;
    }

    public interface ILazoSplatter
    {
        Color MinColor { get; }
        Color MaxColor { get; }
    }

    [Serializable]
    public struct LazoSplatter : ILazoSplatter
    {
        [SerializeField] private Color _minColor;
        [SerializeField] private Color _maxColor;

        public Color MinColor => _minColor;
        public Color MaxColor => _maxColor;
    }
}

