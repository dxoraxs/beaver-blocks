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