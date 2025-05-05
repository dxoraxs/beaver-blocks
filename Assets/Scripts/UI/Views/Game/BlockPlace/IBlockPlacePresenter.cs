using System;
using UniRx;
using UnityEngine;

namespace BeaverBlocks.UI.Views.Game.BlockPlace
{
    public interface IBlockPlacePresenter
    {
        event Action OnBlockPointDown;
        IReadOnlyReactiveProperty<Sprite> BlockIconStream { get; }
        IReadOnlyReactiveProperty<Color> BlockColorStream { get; }
        void OnPointerDown();
        void SetSprite(Sprite sprite);
        void SetColor(Color color);
    }
}