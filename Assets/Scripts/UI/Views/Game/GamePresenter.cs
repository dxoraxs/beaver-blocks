using System.Collections.Generic;
using BeaverBlocks.Core.Cells;
using BeaverBlocks.Core.Game.BlockPlace;
using BeaverBlocks.DI.Factories;
using BeaverBlocks.UI.Views.Game.BottomView;
using BeaverBlocks.UI.Views.Game.GridCells;
using Cysharp.Threading.Tasks;
using UnityEngine.Scripting;

namespace BeaverBlocks.UI.Views.Game
{
    public class GamePresenter : BasePresenter<GameView>, IGamePresenter
    {
        private readonly CellPresenterManager _cellPresenterManager;
        private readonly BlockPlaceManager _blockPlaceManager;
        
        [Preserve]
        protected GamePresenter(IPanelService panelService, IIocFactory iocFactory, CellPresenterManager cellPresenterManager, BlockPlaceManager blockPlaceManager) : base(panelService, iocFactory)
        {
            _cellPresenterManager = cellPresenterManager;
            _blockPlaceManager = blockPlaceManager;
        }

        public IGridCellsPresenter GridCellsPresenter => _iocFactory.Create<GridCellPresenter>();
        public IBottomBlocksPresenter BottomBlocksPresenter => _iocFactory.Create<BottomBlocksPresenter>();
    }
}