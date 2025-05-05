using UnityEngine;

namespace BeaverBlocks.UI.Views.Game.DragBlockObject
{
    public interface IDragBlockObjectPresenter
    {
        Color Color { get; }
        Sprite Sprite { get; }
    }
}