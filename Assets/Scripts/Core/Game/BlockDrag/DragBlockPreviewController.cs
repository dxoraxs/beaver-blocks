using System;
using System.Collections.Generic;
using System.Linq;
using BeaverBlocks.Configs;
using BeaverBlocks.Configs.Data;
using BeaverBlocks.Core.Cells;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;

namespace BeaverBlocks.Core.Game
{
    public class DragBlockPreviewController : IDisposable
    {
        private readonly IConfigsService _configsService;
        private readonly CellModelManager _cellModelManager;
        private readonly DragBlockController _dragBlockController;
        private readonly BlocksDatabase _blocksDatabase;
        private readonly List<(int, int)> _usedCellIndexes = new();
        private BlockConfig _blockConfig;
        private int _blockColor;

        [Preserve]
        public DragBlockPreviewController(CellModelManager cellModelManager, DragBlockController dragBlockController,
            IConfigsService configsService)
        {
            _cellModelManager = cellModelManager;
            _dragBlockController = dragBlockController;
            _configsService = configsService;

            _blocksDatabase = _configsService.Get<BlocksDatabase>();

            _dragBlockController.OnStartMove += OnStartDrag;
            _dragBlockController.OnMove += OnMove;
        }

        private void InitializePreview(string blockId, int groupColor)
        {
            _blockConfig = _blocksDatabase.BlockConfigs.First(block => block.Id == blockId);
            _blockColor = groupColor;
        }

        private void OnStartDrag(DragBlockStartData startData)
        {
            InitializePreview(startData.BlockId, startData.ColorGroup);
            _usedCellIndexes.Clear();
        }

        private void OnMove(DragBlockUpdateData updateData)
        {
            var newList = new (int, int)[_blockConfig.Shape.Length];
            for (var i = 0; i < _blockConfig.Shape.Length; i++)
            {
                newList[i] = (
                    updateData.IndexCell.x + _blockConfig.Shape[i].x,
                    updateData.IndexCell.y + _blockConfig.Shape[i].y
                    );
            }
            
            if (!_cellModelManager.TryGetCellsEmpty(newList, out var cells))
            {
                ClearUsedCells();
                return;
            }
            
            ClearUnusedCells(newList);

            _usedCellIndexes.Clear();
            _usedCellIndexes.AddRange(newList);

            var cellModels = cells.ToArray();
            foreach (var modelKey in cellModels)
            {
                _cellModelManager.SetPreview(modelKey, _blockColor);
            }
        }

        private void ClearUnusedCells((int, int)[] newList)
        {
            var newSet = new HashSet<(int, int)>(newList);
            foreach (var oldIndex in _usedCellIndexes)
            {
                if (!newSet.Contains(oldIndex))
                {
                    if (_cellModelManager.CellModels.TryGetValue(oldIndex, out var oldCell))
                    {
                        oldCell.ClearCell();
                    }
                }
            }
        }

        private void ClearUsedCells()
        {
            foreach (var usedCellIndex in _usedCellIndexes)
            {
                var cellModel = _cellModelManager.CellModels[usedCellIndex];
                cellModel.ClearCell();
            }

            _usedCellIndexes.Clear();
        }

        public void Dispose()
        {
            ClearUsedCells();
            _dragBlockController.OnStartMove -= OnStartDrag;
            _dragBlockController.OnMove -= OnMove;
        }
    }
}