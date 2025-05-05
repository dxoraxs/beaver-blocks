using UniRx;
using UnityEngine;
using UnityEngine.Scripting;

namespace BeaverBlocks.UI.Views.Game.Cells
{
    public class CellPresenter : ICellPresenter
    {
        private readonly BoolReactiveProperty _cellEnable = new();
        private readonly ReactiveProperty<Color> _cellColor = new();

        public IReadOnlyReactiveProperty<bool> CellEnableStream => _cellEnable;
        public IReadOnlyReactiveProperty<Color> CellColorStream => _cellColor;

        public void SetEnable(bool value)
        {
            _cellEnable.Value = value;
        }

        public void SetCellColor(Color color)
        {
            _cellEnable.Value = true;
            _cellColor.Value = color;
        }
    }
}