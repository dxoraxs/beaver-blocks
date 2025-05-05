using System.Collections.Generic;
using System.Linq;
using BeaverBlocks.Configs;
using BeaverBlocks.Configs.Data;
using BeaverBlocks.UI.Views.Game.Cells;
using UniRx;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;

namespace BeaverBlocks.Core.Cells
{
    public class CellPresentersManager
    {
        private readonly List<ICellPresenter> _cellPresenters = new();
        private readonly IConfigsService _configsService;
        private readonly CellColorsConfig _cellColorsConfig;

        public IEnumerable<ICellPresenter> GetCellPresenters => _cellPresenters;
        
        [Preserve]
        public CellPresentersManager(IConfigsService configsService)
        {
            _configsService = configsService;

            _cellColorsConfig = _configsService.Get<CellColorsConfig>();
        }

        public void SetCells(IEnumerable<ICellPresenter> cellPresenters, IEnumerable<CellModel> cellModels)
        {
            var presentersArray = cellPresenters.ToArray();
            var modelsArray = cellModels.ToArray();
            
            _cellPresenters.AddRange(presentersArray);
            
            for (var i = 0; i < presentersArray.Length && i < modelsArray.Length; i++)
            {
                var presenter = presentersArray[i];
                var model = modelsArray[i];
                SubscribePresenterToModel(presenter, model);
            }
        }

        private void SubscribePresenterToModel(ICellPresenter presenter, CellModel cellModel)
        {
            cellModel.CellColorStream.Subscribe(index => presenter.SetCellColor(GetColor(index).DefaultColors));
            cellModel.PreviewColorStream.Subscribe(index => presenter.SetCellColor(GetColor(index).PreviewColors));
            cellModel.IsBusyStream.Subscribe(presenter.SetEnable);
        }

        private PairColorValue GetColor(int index)
        {
            return _cellColorsConfig.CellColors[index];
        }
    }
}