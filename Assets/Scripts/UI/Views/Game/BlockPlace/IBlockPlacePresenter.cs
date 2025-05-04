using UniRx;
using UnityEngine;

namespace BeaverBlocks.UI.Views.Game.BlockPlace
{
    public interface IBlockPlacePresenter
    {
        IReadOnlyReactiveProperty<Sprite> BlockIconStream { get; }
        void OnPointerDown();
    }
}