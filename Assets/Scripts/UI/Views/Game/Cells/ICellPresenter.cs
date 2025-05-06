using UniRx;
using UnityEngine;

namespace BeaverBlocks.UI.Views.Game.Cells
{
    public interface ICellPresenter
    {
        IReadOnlyReactiveProperty<bool> CellEnableStream { get; }
        IReadOnlyReactiveProperty<bool> PreviewEnableStream { get; }
        IReadOnlyReactiveProperty<Color> CellColorStream { get; }
        IReadOnlyReactiveProperty<Color> PreviewColorStream { get; }
        
        void SetCellColor(Color color);
        void SetActive(bool value);
        void SetPreviewColor(Color color);
        void SetPreviewActive(bool value);
    }
}