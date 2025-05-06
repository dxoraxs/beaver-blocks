using UniRx;
using UnityEngine;

namespace BeaverBlocks.Core.Cells
{
    public class CellModel
    {
        private readonly BoolReactiveProperty _isBusy = new();
        private readonly BoolReactiveProperty _isPreviewEnable = new();
        private readonly IntReactiveProperty _cellColorGroup = new(-1);
        private readonly IntReactiveProperty _previewColorGroup = new(-1);
        private int _defaultColorGroup;

        public IReadOnlyReactiveProperty<bool> IsBusyStream => _isBusy;
        public IReadOnlyReactiveProperty<bool> IsPreviewStream => _isPreviewEnable;
        public IReadOnlyReactiveProperty<int> CellColorStream => _cellColorGroup;
        public IReadOnlyReactiveProperty<int> PreviewColorStream => _previewColorGroup;

        public void SetBusy(int colorGroup)
        {
            _defaultColorGroup = colorGroup;
            _cellColorGroup.Value = colorGroup;
            _isBusy.Value = true;
        }

        public void SetOverrideColor(int colorGroup)
        {
            if (colorGroup < 0)
                _cellColorGroup.Value = _defaultColorGroup;
            else
                _cellColorGroup.Value = colorGroup;
        }

        public void SetPreview(int colorGroup)
        {
            _previewColorGroup.Value = colorGroup;
            _isPreviewEnable.Value = true;
        }

        public void ClearCell()
        {
            _cellColorGroup.Value = -1;
            _previewColorGroup.Value = -1;
            _isBusy.Value = false;
            _isPreviewEnable.Value = false;
        }
    }
}