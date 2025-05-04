using System.Collections.Generic;
using BeaverBlocks.UI.Views.Game.Cells;

namespace BeaverBlocks.UI.Views.Game.GridCells
{
    public interface IGridCellsPresenter
    {
        CellView GetCellViewPrefab { get; }
        uint SizeGrid { get; }
        IEnumerable<ICellPresenter> GetCellPresenters { get; }
    }
}