using System.Collections.Generic;
using BeaverBlocks.Core.Cells;
using BeaverBlocks.DI.Factories;
using BeaverBlocks.UI.Views.Game.GridCells;
using Cysharp.Threading.Tasks;
using UnityEngine.Scripting;

namespace BeaverBlocks.UI.Views.Game
{
    public class GamePresenter : BasePresenter<GameView>, IGamePresenter
    {
        private readonly CellsManager _cellsManager;
        
        [Preserve]
        protected GamePresenter(IPanelService panelService, IIocFactory iocFactory, CellsManager cellsManager) : base(panelService, iocFactory)
        {
            _cellsManager = cellsManager;
        }

        public IGridCellsPresenter GridCellsPresenter => _iocFactory.Create<GridCellPresenter>();
    }
}