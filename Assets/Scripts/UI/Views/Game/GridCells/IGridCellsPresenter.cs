using System;
using System.Collections.Generic;
using BeaverBlocks.UI.Views.Game.Cells;

namespace BeaverBlocks.UI.Views.Game.GridCells
{
    public interface IGridCellsPresenter
    {
        event Action OnPointerEnterEvent;
        CellView GetCellViewPrefab { get; }
        uint SizeGrid { get; }
        IEnumerable<ICellPresenter> GetCellPresenters { get; }
        void OnPointerEnter();
    }
}