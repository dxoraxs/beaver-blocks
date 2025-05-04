using BeaverBlocks.Core.Cells;
using BeaverBlocks.UI.Views.Game.GridCells;
using UnityEngine;

namespace BeaverBlocks.UI.Views.Game
{
    public class GameView : BaseView
    {
        [SerializeField] private GridCellsView _gridCellsView;
        private IGamePresenter _gamePresenter;
 
        public void Initialize(IGamePresenter gamePresenter)
        {
            _gamePresenter = gamePresenter;
            
            _gridCellsView.Initialize(_gamePresenter.GridCellsPresenter);
        }
    }
}