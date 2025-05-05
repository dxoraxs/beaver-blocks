using System;
using System.Collections.Generic;
using BeaverBlocks.Configs;
using BeaverBlocks.Configs.Data;
using BeaverBlocks.Core.Cells;
using BeaverBlocks.UI.Views.Game.Cells;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;
using VContainer.Unity;

namespace BeaverBlocks.UI.Views.Game.GridCells
{
    public class GridCellPresenter : IGridCellsPresenter
    {
        public event Action OnPointerEnterEvent;
        private readonly IConfigsService _configsService;
        private readonly GameSettings _gameSettings;
        private readonly PrefabsConfig _prefabsConfig;
        private readonly CellPresentersManager _cellPresentersManager;
        
        public CellView GetCellViewPrefab => _prefabsConfig.CellView;
        public uint SizeGrid => _gameSettings.GridSize;
        public IEnumerable<ICellPresenter> GetCellPresenters => _cellPresentersManager.GetCellPresenters;

        [Preserve]
        public GridCellPresenter(IConfigsService configsService, CellPresentersManager cellPresentersManager)
        {
            _configsService = configsService;
            _cellPresentersManager = cellPresentersManager;
            
            _gameSettings = _configsService.Get<GameSettings>();
            _prefabsConfig = _configsService.Get<PrefabsConfig>();
        }

        public void OnPointerEnter()
        {
            OnPointerEnterEvent?.Invoke();
        }
    }
}