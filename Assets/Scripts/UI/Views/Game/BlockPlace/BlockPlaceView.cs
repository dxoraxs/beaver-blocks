using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BeaverBlocks.UI.Views.Game.BlockPlace
{
    public class BlockPlaceView : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Image _image;
        private IBlockPlacePresenter _presenter;

        public void Initialize(IBlockPlacePresenter presenter)
        {
            _presenter = presenter;

            _presenter.BlockIconStream.Subscribe(SetSprite).AddTo(this);
            _presenter.BlockColorStream.Subscribe(SetColor).AddTo(this);
        }

        private void SetSprite(Sprite sprite)
        {
            _image.enabled = sprite != null;
            _image.sprite = sprite;
        }

        private void SetColor(Color color)
        {
            _image.color = color;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _presenter.OnPointerDown();
        }
    }
}