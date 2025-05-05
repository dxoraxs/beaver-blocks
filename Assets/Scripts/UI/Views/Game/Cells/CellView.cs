using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace BeaverBlocks.UI.Views.Game.Cells
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private Image _cellImage;
        private ICellPresenter _presenter;

        public void Initialize(ICellPresenter presenter)
        {
            _presenter = presenter;

            _presenter.CellEnableStream.Subscribe(SetEnableCell).AddTo(this);
            _presenter.CellColorStream.Subscribe(SetColorToCell).AddTo(this);
        }

        private void SetEnableCell(bool value)
        {
            var newColor = _cellImage.color;
            newColor.a = value ? 1 : 0;
            _cellImage.color = newColor;
        }

        private void SetColorToCell(Color color)
        {
            color.a = _cellImage.color.a;
            _cellImage.color = color;
        }
    }
}