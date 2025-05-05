using UnityEngine;

namespace BeaverBlocks.UI.Views.Game.DragBlockObject
{
    public class DragBlockObjectPresenter : IDragBlockObjectPresenter
    {
        public Color Color { get; private set; }

        public DragBlockObjectPresenter(Color color)
        {
            Color = color;
        }
    }
}