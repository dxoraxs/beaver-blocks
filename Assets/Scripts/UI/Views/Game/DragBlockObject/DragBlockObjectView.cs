using UnityEngine;
using UnityEngine.UI;

namespace BeaverBlocks.UI.Views.Game.DragBlockObject
{
    public class DragBlockObjectView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        private IDragBlockObjectPresenter _presenter;

        public void Initialize(IDragBlockObjectPresenter presenter)
        {
            _presenter = presenter;
            
            _image.color = _presenter.Color;
            _image.sprite = _presenter.Sprite;
        }
    }
}