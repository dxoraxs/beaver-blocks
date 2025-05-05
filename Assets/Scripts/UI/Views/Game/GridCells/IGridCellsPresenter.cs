using System;
using System.Collections.Generic;
using BeaverBlocks.UI.Views.Game.Cells;
using UnityEngine;

namespace BeaverBlocks.UI.Views.Game.GridCells
{
    public interface IGridCellsPresenter
    {
        CellView GetCellViewPrefab { get; }
        uint SizeGrid { get; }
        IEnumerable<ICellPresenter> GetCellPresenters { get; }
        void SetRectTransform(RectTransform rectTransform);
        bool IsPointInsideUIElement(Vector2 screenPoint);
        (int x, int y) GetGridIndexFromScreenPoint(Vector2 point); 
    }
}