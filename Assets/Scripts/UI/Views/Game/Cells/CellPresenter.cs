using UniRx;
using UnityEngine;
using UnityEngine.Scripting;

namespace BeaverBlocks.UI.Views.Game.Cells
{
    public class CellPresenter : ICellPresenter
    {
        private readonly BoolReactiveProperty _blockEnable = new();
        private readonly BoolReactiveProperty _previewEnable = new();
        private readonly ReactiveProperty<Color> _blockColor = new();
        private readonly ReactiveProperty<Color> _previewColor = new();
        
        public IReadOnlyReactiveProperty<bool> EnableBlockStream => _blockEnable;
        public IReadOnlyReactiveProperty<bool> EnablePreviewBlockStream => _previewEnable;
        public IReadOnlyReactiveProperty<Color> BlockColorStream => _blockColor;
        public IReadOnlyReactiveProperty<Color> PreviewBlockColorStream => _previewColor;

        public void SetPreviewColor(Color color)
        {
            _previewEnable.Value = true;
            _previewColor.Value = color;
        }

        public void SetBlockColor(Color color)
        {
            _blockEnable.Value = true;
            _blockColor.Value = color;
        }

        public void ClearPreview()
        {
            _previewEnable.Value = false;
        }

        public void ClearBlock()
        {
            _blockEnable.Value = false;
        }
    }
}