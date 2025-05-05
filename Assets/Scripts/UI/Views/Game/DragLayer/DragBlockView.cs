using UnityEngine;

namespace BeaverBlocks.UI.Views.Game.DragLayer
{
    public class DragBlockView : MonoBehaviour
    {
        private IDragBlockPresenter _presenter;

        public void Initialize(IDragBlockPresenter presenter)
        {
            _presenter = presenter;
            
            _presenter.SetParent(transform);
        }
    }
}