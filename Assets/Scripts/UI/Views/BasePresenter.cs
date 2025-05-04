using BeaverBlocks.DI.Factories;

namespace BeaverBlocks.UI.Views
{
    public class BasePresenter<TView> where TView : BaseView
    {
        protected readonly IIocFactory _iocFactory;
        protected readonly IPanelService PanelService;
        protected readonly TView View;

        protected BasePresenter(IPanelService panelService, IIocFactory iocFactory)
        {
            PanelService = panelService;
            _iocFactory = iocFactory;
            View = PanelService.Get<TView>();
        }
    }
}