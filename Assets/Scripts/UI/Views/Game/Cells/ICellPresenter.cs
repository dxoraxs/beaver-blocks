using UniRx;
using UnityEngine;

namespace BeaverBlocks.UI.Views.Game.Cells
{
    public interface ICellPresenter
    {
        IReadOnlyReactiveProperty<bool> EnableBlockStream { get; }
        IReadOnlyReactiveProperty<bool> EnablePreviewBlockStream { get; }
        IReadOnlyReactiveProperty<Color> BlockColorStream { get; }
        IReadOnlyReactiveProperty<Color> PreviewBlockColorStream { get; }
        
        void SetPreviewColor(Color color);
        void SetBlockColor(Color color);
        void ClearPreview();
        void ClearBlock();
    }
}