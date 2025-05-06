using UniRx;
using UnityEngine;
using UnityEngine.Scripting;

namespace BeaverBlocks.UI.Views.Game.Cells
{
    public class CellPresenter : ICellPresenter
    {
        private readonly BoolReactiveProperty _cellEnable = new();
        private readonly BoolReactiveProperty _previewEnable = new();
        private readonly ReactiveProperty<Color> _cellColor = new();
        private readonly ReactiveProperty<Color> _previewColor = new();

        public IReadOnlyReactiveProperty<bool> CellEnableStream => _cellEnable;
        public IReadOnlyReactiveProperty<bool> PreviewEnableStream => _previewEnable;
        public IReadOnlyReactiveProperty<Color> CellColorStream => _cellColor;
        public IReadOnlyReactiveProperty<Color> PreviewColorStream => _previewColor;

        public void SetActive(bool value)
        {
            _cellEnable.Value = value;
        }

        public void SetCellColor(Color color)
        {
            _cellColor.Value = color;
        }

        public void SetPreviewActive(bool value)
        {
            _previewEnable.Value = value;
        }

        public void SetPreviewColor(Color color)
        {
            _previewColor.Value = color;
        }
    }
}