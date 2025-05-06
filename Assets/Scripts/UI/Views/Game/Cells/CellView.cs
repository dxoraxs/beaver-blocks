using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace BeaverBlocks.UI.Views.Game.Cells
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private Image _cellImage;
        [SerializeField] private Image _previewImage;
        private ICellPresenter _presenter;

        public void Initialize(ICellPresenter presenter)
        {
            _presenter = presenter;

            _presenter.CellEnableStream.Subscribe(SetEnableCell).AddTo(this);
            _presenter.CellColorStream.Subscribe(SetColorToCell).AddTo(this);
            _presenter.PreviewEnableStream.Subscribe(SetPreviewEnableCell).AddTo(this);
            _presenter.PreviewColorStream.Subscribe(SetPreviewColorToCell).AddTo(this);
        }

        private void SetEnableCell(bool value)
        {
            _cellImage.enabled = value;
        }

        private void SetColorToCell(Color color)
        {
            _cellImage.color = color;
        }

        private void SetPreviewEnableCell(bool value)
        {
            _previewImage.enabled = value;
        }

        private void SetPreviewColorToCell(Color color)
        {
            _previewImage.color = color;
        }
    }
}