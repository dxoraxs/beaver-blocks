using System.Collections.Generic;
using BeaverBlocks.UI.Views.Game.BlockPlace;

namespace BeaverBlocks.UI.Views.Game.BottomView
{
    public interface IBottomBlocksPresenter
    {
        BlockPlaceView PlaceViewPrefab { get; }
        IEnumerable<IBlockPlacePresenter> BottomPlacePresenters { get; }
    }
}