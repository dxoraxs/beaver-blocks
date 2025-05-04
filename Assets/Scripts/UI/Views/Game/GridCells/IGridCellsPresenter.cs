using System.Collections.Generic;
using BeaverBlocks.UI.Views.Game.Cells;

namespace BeaverBlocks.UI.Views.Game.GridCells
{
    public interface IGridCellsPresenter
    {
        public CellView GetCellViewPrefab { get; }
        public uint SizeGrid { get; }
        public IEnumerable<ICellPresenter> GetCellPresenters { get; }
    }
}