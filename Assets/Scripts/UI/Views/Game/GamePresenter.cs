using System.Collections.Generic;
using BeaverBlocks.Core.Cells;
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
        private readonly CellPresenterManager _cellPresenterManager;
        private readonly BlockPlaceModelManager _blockPlaceModelManager;
        
        [Preserve]
        protected GamePresenter(IPanelService panelService, IIocFactory iocFactory, CellPresenterManager cellPresenterManager, BlockPlaceModelManager blockPlaceModelManager) : base(panelService, iocFactory)
        {
            _cellPresenterManager = cellPresenterManager;
            _blockPlaceModelManager = blockPlaceModelManager;
        }

        public IGridCellsPresenter GridCellsPresenter => _iocFactory.Create<GridCellPresenter>();
        public IBottomBlocksPresenter BottomBlocksPresenter => _iocFactory.Create<BottomBlocksPresenter>();
        public IDragBlockPresenter DragBlockPresenter => _iocFactory.Create<DragBlockPresenter>();
    }
}