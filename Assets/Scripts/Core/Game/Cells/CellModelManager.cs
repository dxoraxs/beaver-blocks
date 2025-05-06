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

        [Preserve]
        public CellModelManager()
        {
        }
        public IEnumerable<CellModel> TryGetAllCellsFromRowAndColumn(IEnumerable<(int x, int y)> newCellIndexes)
        {
            var foundRows = new HashSet<int>();
            var foundColumns = new HashSet<int>();
            var result = new HashSet<CellModel>();

            foreach (var (x, y) in newCellIndexes)
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
                    if (!IsCellBusy((x, y)))
                        return false;
                }
                return true;
            }

            bool IsColumnFull(int x)
            {
                for (var y = 0; _cellModels.ContainsKey((x, y)); y++)
                {
                    if (!IsCellBusy((x, y)))
                        return false;
                }
                return true;
            }

            void AddRowCells(int y, HashSet<CellModel> target)
            {
                for (var x = 0; _cellModels.TryGetValue((x, y), out var cell); x++)
                {
                    target.Add(cell);
                }
            }

            void AddColumnCells(int x, HashSet<CellModel> target)
            {
                for (var y = 0; _cellModels.TryGetValue((x, y), out var cell); y++)
                {
                    target.Add(cell);
                }
            }
        }
        
        public bool TryGetCellsEmpty(IEnumerable<(int x, int y)> cellIndex, out IEnumerable<CellModel> cells)
        {
            var offsetIndices = cellIndex.ToArray();
            var cellModels = new List<CellModel>(offsetIndices.Length);
            cells = Array.Empty<CellModel>();

            foreach (var offsetIndex in offsetIndices)
            {
                if (!CellModels.ContainsKey(offsetIndex) || IsCellBusy(offsetIndex))
                    return false;

                cellModels.Add(CellModels[offsetIndex]);
            }

            cells = cellModels;
            return true;
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