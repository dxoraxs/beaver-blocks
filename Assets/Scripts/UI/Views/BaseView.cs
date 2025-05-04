using UnityEngine;

namespace BeaverBlocks.UI.Views
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BaseView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        public void SetEnabled(bool value)
        {
            _canvasGroup.interactable = value;
            _canvasGroup.blocksRaycasts = value;
            _canvasGroup.alpha = value ? 1 : 0;
        }
    }
}