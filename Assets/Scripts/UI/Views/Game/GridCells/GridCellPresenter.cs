using System.Collections.Generic;
using BeaverBlocks.Configs;
using BeaverBlocks.Configs.Data;
using BeaverBlocks.Core.Cells;
using BeaverBlocks.UI.Views.Game.Cells;
using UnityEngine;
using UnityEngine.Scripting;
using VContainer.Unity;

namespace BeaverBlocks.UI.Views.Game.GridCells
{
    public class GridCellPresenter : IGridCellsPresenter
    {
        private readonly IConfigsService _configsService;
        private readonly GameSettings _gameSettings;
        private readonly PrefabsConfig _prefabsConfig;
        private readonly CellPresenterManager _cellPresenterManager;
        
        public CellView GetCellViewPrefab => _prefabsConfig.CellView;
        public uint SizeGrid => _gameSettings.GridSize;
        public IEnumerable<ICellPresenter> GetCellPresenters => _cellPresenterManager.GetCellPresenters;

        [Preserve]
        public GridCellPresenter(IConfigsService configsService, CellPresenterManager cellPresenterManager)
        {
            _configsService = configsService;
            _cellPresenterManager = cellPresenterManager;
            
            _gameSettings = _configsService.Get<GameSettings>();
            _prefabsConfig = _configsService.Get<PrefabsConfig>();
        }
    }
}