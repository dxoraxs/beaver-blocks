using BeaverBlocks.UI.Views.Game.BottomView;
using BeaverBlocks.UI.Views.Game.DragLayer;
using BeaverBlocks.UI.Views.Game.GridCells;
using Cysharp.Threading.Tasks;

namespace BeaverBlocks.UI.Views.Game
{
    public interface IGamePresenter
    {
        IGridCellsPresenter GridCellsPresenter { get; }
        IBottomBlocksPresenter BottomBlocksPresenter { get; }
        IDragBlockPresenter DragBlockPresenter { get; }
    }
}