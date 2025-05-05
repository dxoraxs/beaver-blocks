using UnityEngine;

namespace BeaverBlocks.UI.Views.Game.DragLayer
{
    public interface IDragBlockPresenter
    {
        void SetParent(Transform parent);
        void StartMove(Vector2 startPosition, string blockId, int groupColor);
        void StopMove();
    }
}