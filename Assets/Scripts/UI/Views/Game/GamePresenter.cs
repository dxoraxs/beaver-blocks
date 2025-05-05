using System.Collections.Generic;
using BeaverBlocks.Core.Cells;
using BeaverBlocks.Core.Game;
using BeaverBlocks.Core.Game.BlockPlace;
using BeaverBlocks.DI.Factories;
using BeaverBlocks.UI.Views.Game.BottomView;
using BeaverBlocks.UI.Views.Game.DragLayer;
using BeaverBlocks.UI.Views.Game.GridCells;
using Cysharp.Threading.Tasks;
using UnityEngine.Scripting;

namespace BeaverBlocks.UI.Views.Game
{
    public class GamePresenter : BasePresenter<GameView>, IGamePresenter
    {
        private readonly CellPresentersManager _cellPresentersManager;
        private readonly BlockPlacePresenterManager _blockPlacePresenterManager;
        private readonly DragBlockController _dragBlockController;

        [Preserve]
        protected GamePresenter(IPanelService panelService, IIocFactory iocFactory,
            CellPresentersManager cellPresentersManager, BlockPlacePresenterManager blockPlacePresenterManager, DragBlockController dragBlockController) : base(
            panelService, iocFactory)
        {
            _cellPresentersManager = cellPresentersManager;
            _blockPlacePresenterManager = blockPlacePresenterManager;
            _dragBlockController = dragBlockController;
        }

        public IGridCellsPresenter GridCellsPresenter =>
            _iocFactory.Create<GridCellPresenter, CellPresentersManager>(_cellPresentersManager);

        public IBottomBlocksPresenter BottomBlocksPresenter =>
            _iocFactory.Create<BottomBlocksPresenter, BlockPlacePresenterManager>(_blockPlacePresenterManager);

        public IDragBlockPresenter DragBlockPresenter => _iocFactory.Create<DragBlockPresenter, DragBlockController>(_dragBlockController);
    }
}