using BeaverBlocks.UI.Views.Game.BottomView;
using BeaverBlocks.UI.Views.Game.DragLayer;
using BeaverBlocks.UI.Views.Game.GridCells;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BeaverBlocks.UI.Views.Game
{
    public interface IGamePresenter
    {
        bool IsGridRaycast(Vector2 point);
        (int x, int y) GetGridIndexFromScreenPoint(Vector2 point); 
        IGridCellsPresenter GridCellsPresenter { get; }
        IBottomBlocksPresenter BottomBlocksPresenter { get; }
        IDragBlockPresenter DragBlockPresenter { get; }
    }
}