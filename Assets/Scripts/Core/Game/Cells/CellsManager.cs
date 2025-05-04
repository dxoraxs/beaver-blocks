using System.Collections.Generic;
using BeaverBlocks.UI.Views.Game.Cells;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine.Scripting;

namespace BeaverBlocks.Core.Cells
{
    public class CellsManager
    {
        private readonly Dictionary<(int x, int y), ICellPresenter> _cellPresenters = new();

        public IReadOnlyDictionary<(int x, int y), ICellPresenter> CellPresenters => _cellPresenters;
        public IEnumerable<ICellPresenter> GetCellPresenters => _cellPresenters.Values;
        
        [Preserve]
        public CellsManager()
        {
        }

        public void SetCells(IReadOnlyDictionary<(int x, int y), ICellPresenter> cellPresenters)
        {
            _cellPresenters.AddRange(cellPresenters);
        }
    }
}