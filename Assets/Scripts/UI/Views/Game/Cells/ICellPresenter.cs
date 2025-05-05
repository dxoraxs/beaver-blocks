using UniRx;
using UnityEngine;

namespace BeaverBlocks.UI.Views.Game.Cells
{
    public interface ICellPresenter
    {
        IReadOnlyReactiveProperty<bool> CellEnableStream { get; }
        IReadOnlyReactiveProperty<Color> CellColorStream { get; }
        
        void SetCellColor(Color color);
        void SetEnable(bool value);
    }
}