using BeaverBlocks.UI.Views;
using Cysharp.Threading.Tasks;

namespace BeaverBlocks.UI
{
    public interface IPanelService
    {
        void Initialize();
        TView Get<TView>() where TView : BaseView;
    }
}