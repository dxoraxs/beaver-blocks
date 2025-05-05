using UniRx;
using UnityEngine;

namespace BeaverBlocks.Core.Cells
{
    public class CellModel
    {
        private readonly BoolReactiveProperty _isBusy = new();
        private readonly IntReactiveProperty _cellColorGroup = new();
        private readonly IntReactiveProperty _previewColorGroup = new();
        
        public IReadOnlyReactiveProperty<bool> IsBusyStream => _isBusy;
        public IReadOnlyReactiveProperty<int> CellColorStream => _cellColorGroup;
        public IReadOnlyReactiveProperty<int> PreviewColorStream => _previewColorGroup;

        public void SetBusy(int colorGroup)
        {
            _isBusy.Value = true;
            _cellColorGroup.Value = colorGroup;
        }

        public void SetPreview(int colorGroup)
        {
            _previewColorGroup.Value = colorGroup;
        }

        public void ClearCell()
        {
            _isBusy.Value = false;
            _cellColorGroup.Value = 0;
            _previewColorGroup.Value = 0;
        }
    }
}