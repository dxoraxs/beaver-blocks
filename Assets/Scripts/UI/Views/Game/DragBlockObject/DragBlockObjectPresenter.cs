using UnityEngine;

namespace BeaverBlocks.UI.Views.Game.DragBlockObject
{
    public class DragBlockObjectPresenter : IDragBlockObjectPresenter
    {
        public Color Color { get; }
        public Sprite Sprite { get; }

        public DragBlockObjectPresenter(Color color, Sprite sprite)
        {
            Color = color;
            Sprite = sprite;
        }
    }
}