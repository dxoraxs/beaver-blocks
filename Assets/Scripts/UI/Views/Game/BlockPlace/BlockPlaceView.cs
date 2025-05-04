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

            _presenter.BlockIconStream.Subscribe(SetBlockSprite).AddTo(this);
        }

        private void SetBlockSprite(Sprite sprite)
        {
            _image.enabled = sprite != null;
            _image.sprite = sprite;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _presenter.OnPointerDown();
        }
    }
}