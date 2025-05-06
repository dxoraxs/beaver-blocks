using System.Collections.Generic;
using BeaverBlocks.Core.Cells;
using BeaverBlocks.Core.Game;
using BeaverBlocks.Core.Game.BlockPlace;
using BeaverBlocks.DI.Factories;
using BeaverBlocks.UI.Views.Game.BottomView;
using BeaverBlocks.UI.Views.Game.DragLayer;
using BeaverBlocks.UI.Views.Game.GridCells;
using BeaverBlocks.UI.Views.Game.ScoreCounter;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting;

namespace BeaverBlocks.UI.Views.Game
{
    public class GamePresenter : BasePresenter<GameView>, IGamePresenter
    {
        private readonly CellPresentersManager _cellPresentersManager;
        private readonly BlockPlacePresenterManager _blockPlacePresenterManager;
        private readonly DragBlockController _dragBlockController;
        private readonly IGridCellsPresenter _gridCellsPresenter;

        public IGridCellsPresenter GridCellsPresenter => _gridCellsPresenter;

        public IBottomBlocksPresenter BottomBlocksPresenter =>
            _iocFactory.Create<BottomBlocksPresenter, BlockPlacePresenterManager>(_blockPlacePresenterManager);

        public IDragBlockPresenter DragBlockPresenter => _iocFactory.Create<DragBlockPresenter, DragBlockController>(_dragBlockController);
        public IScoreCounterPresenter ScoreCounterPresenter => _iocFactory.Create<ScoreCounterPresenter>();

        [Preserve]
        protected GamePresenter(IPanelService panelService, IIocFactory iocFactory,
            CellPresentersManager cellPresentersManager, BlockPlacePresenterManager blockPlacePresenterManager, DragBlockController dragBlockController) : base(
            panelService, iocFactory)
        {
            _cellPresentersManager = cellPresentersManager;
            _blockPlacePresenterManager = blockPlacePresenterManager;
            _dragBlockController = dragBlockController;

            _gridCellsPresenter = _iocFactory.Create<GridCellPresenter, CellPresentersManager>(_cellPresentersManager);
            
            _dragBlockController.SetFuncGetCellIndex(GetGridIndexFromScreenPoint, _gridCellsPresenter.SizeCell);
        }

        public bool IsGridRaycast(Vector2 point)
        {
            return _gridCellsPresenter.IsPointInsideUIElement(point);
        }

        public (int x, int y) GetGridIndexFromScreenPoint(Vector2 point)
        {
            return _gridCellsPresenter.GetGridIndexFromScreenPoint(point);
        }
    }
}