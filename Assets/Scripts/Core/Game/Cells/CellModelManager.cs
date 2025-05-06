using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;

namespace BeaverBlocks.Core.Cells
{
    public class CellModelManager
    {
        private readonly Dictionary<(int, int), CellModel> _cellModels = new();
        public IReadOnlyDictionary<(int, int), CellModel> CellModels => _cellModels;

        public int CellBusyCounter { get; private set; }

        [Preserve]
        public CellModelManager()
        {
        }
        
        public IEnumerable<(int,int)> GetAllCellsFromRowAndColumn(IEnumerable<(int x, int y)> newCellIndexes)
        {
            var foundRows = new HashSet<int>();
            var foundColumns = new HashSet<int>();
            var result = new HashSet<(int,int)>();

            var cellIndexes = newCellIndexes.ToList();
            foreach (var (x, y) in cellIndexes)
            {
                if (foundRows.Add(y) && IsRowFull(y))
                {
                    AddRowCells(y, result);
                }

                if (foundColumns.Add(x) && IsColumnFull(x))
                {
                    AddColumnCells(x, result);
                }
            }

            return result;

            bool IsRowFull(int y)
            {
                for (var x = 0; _cellModels.ContainsKey((x, y)); x++)
                {
                    if (!IsCellBusy((x, y)) && !cellIndexes.Contains((x,y)))
                        return false;
                }
                return true;
            }

            bool IsColumnFull(int x)
            {
                for (var y = 0; _cellModels.ContainsKey((x, y)); y++)
                {
                    if (!IsCellBusy((x, y)) && !cellIndexes.Contains((x,y)))
                        return false;
                }
                return true;
            }

            void AddRowCells(int y, HashSet<(int,int)> target)
            {
                for (var x = 0; _cellModels.ContainsKey((x, y)); x++)
                {
                    target.Add((x, y));
                }
            }

            void AddColumnCells(int x, HashSet<(int,int)> target)
            {
                for (var y = 0; _cellModels.ContainsKey((x, y)); y++)
                {
                    target.Add((x, y));
                }
            }
        }
        
        public bool TryGetCellsEmpty(IEnumerable<(int x, int y)> cellIndex)
        {
            var offsetIndices = cellIndex.ToArray();

            foreach (var offsetIndex in offsetIndices)
            {
                if (!CellModels.ContainsKey(offsetIndex) || IsCellBusy(offsetIndex))
                    return false;
            }
            return true;
        }

        public void SetBusy((int, int) key, int colorGroup)
        {
            if (_cellModels[key].IsBusyStream.Value) return;

            CellBusyCounter++;
            SetColor(key,colorGroup);
        }
        
        public void SetColor((int, int) key, int colorGroup)
        {
            _cellModels[key].SetBusy(colorGroup);
        }

        public void SetPreview((int, int) key, int colorGroup)
        {
            _cellModels[key].SetPreview(colorGroup);
        }

        public void ClearPreview((int, int) key)
        {
            _cellModels[key].ClearCell();
        }

        public void SetOverride((int, int) key, int colorGroup)
        {
            _cellModels[key].SetOverrideColor(colorGroup);
        }

        public void ClearAll()
        {
            foreach (var cellModel in _cellModels.Values)
            {
                cellModel.ClearCell();
            }

            CellBusyCounter = 0;
        }

        public void ClearCell((int, int) key)
        {
            if (!_cellModels[key].IsBusyStream.Value) return;

            CellBusyCounter--;
            _cellModels[key].ClearCell();
        }
        
        public bool IsCellBusy((int, int) key)
        {
            return _cellModels[key].IsBusyStream.Value;
        }

        public void SetModels(IReadOnlyDictionary<(int, int), CellModel> models)
        {
            _cellModels.AddRange(models);
        }
    }
}