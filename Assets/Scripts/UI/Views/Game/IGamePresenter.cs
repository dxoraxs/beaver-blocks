using BeaverBlocks.UI.Views.Game.GridCells;
using Cysharp.Threading.Tasks;

namespace BeaverBlocks.UI.Views.Game
{
    public interface IGamePresenter
    {
        IGridCellsPresenter GridCellsPresenter { get; }
    }
}