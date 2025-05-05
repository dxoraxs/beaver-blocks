using UnityEngine;

namespace BeaverBlocks.UI.Views.Game.DragBlockObject
{
    public class DragBlockObjectView : MonoBehaviour
    {
        private IDragBlockObjectPresenter _presenter;

        public void Initialize(IDragBlockObjectPresenter presenter)
        {
            _presenter = presenter;
        }
    }
}