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
        private readonly List<(int, int)> _overrideCellIndexes = new();
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
            var newList = updateData.IndexCell;

            var overrideCells = _cellModelManager.GetAllCellsFromRowAndColumn(newList).ToArray();

            ClearUnusedCells(newList);
            ClearOverrideCells(overrideCells);

            _usedCellIndexes.Clear();
            _overrideCellIndexes.Clear();

            _usedCellIndexes.AddRange(newList);
            _overrideCellIndexes.AddRange(overrideCells);

            foreach (var overrideCellIndex in _overrideCellIndexes)
            {
                _cellModelManager.SetOverride(overrideCellIndex, _blockColor);
            }

            foreach (var modelKey in _usedCellIndexes)
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
                    _cellModelManager.ClearPreview(oldIndex);
                }
            }
        }

        private void ClearOverrideCells((int, int)[] newList)
        {
            var newSet = new HashSet<(int, int)>(newList);
            foreach (var oldIndex in _overrideCellIndexes)
            {
                if (!newSet.Contains(oldIndex))
                {
                    _cellModelManager.SetOverride(oldIndex, -1);
                }
            }
        }

        private void ClearUsedCells()
        {
            foreach (var oldIndex in _overrideCellIndexes)
            {
                _cellModelManager.SetOverride(oldIndex, -1);
            }

            foreach (var usedCellIndex in _usedCellIndexes)
            {
                _cellModelManager.ClearPreview(usedCellIndex);
            }

            _overrideCellIndexes.Clear();
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