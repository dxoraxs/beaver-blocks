using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace BeaverBlocks.UI.Views.Game.Cells
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private Image _blockImage;
        [SerializeField] private Image _previewBlockImage;
        private ICellPresenter _presenter;

        public void Initialize(ICellPresenter presenter)
        {
            _presenter = presenter;

            _presenter.EnableBlockStream.Subscribe(SetEnableBlockImage).AddTo(this);
            _presenter.EnablePreviewBlockStream.Subscribe(SetEnablePreviewBlockImage).AddTo(this);

            _presenter.BlockColorStream.Subscribe(SetColorToBlockImage).AddTo(this);
            _presenter.PreviewBlockColorStream.Subscribe(SetColorToPreviewBlockImage).AddTo(this);
        }

        private void SetEnableBlockImage(bool value)
        {
            var newColor = _blockImage.color;
            newColor.a = value ? 1 : 0;
            _blockImage.color = newColor;
        }

        private void SetEnablePreviewBlockImage(bool value)
        {
            var newColor = _previewBlockImage.color;
            newColor.a = value ? 1 : 0;
            _previewBlockImage.color = newColor;
        }

        private void SetColorToBlockImage(Color color)
        {
            color.a = _blockImage.color.a;
            _blockImage.color = color;
        }

        private void SetColorToPreviewBlockImage(Color color)
        {
            color.a = _previewBlockImage.color.a;
            _previewBlockImage.color = color;
        }
    }
}