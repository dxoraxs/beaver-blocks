using System.Collections.Generic;
using BeaverBlocks.UI.Views.Game.Cells;

namespace BeaverBlocks.Core.Cells
{
    public class CellsInstaller
    {
        private readonly uint _sizeGrid;
        private readonly Dictionary<(int, int), ICellPresenter> _cellPresenters = new(); 
        
        public IReadOnlyDictionary<(int, int), ICellPresenter> CellPresenters => _cellPresenters;
        
        public CellsInstaller(uint sizeGrid)
        {
            _sizeGrid = sizeGrid;
        }

        public void Initialize()
        {
            for (var x = 0; x < _sizeGrid; x++)
            {
                for (var y = 0; y < _sizeGrid; y++)
                {
                    _cellPresenters.Add((x, y), new CellPresenter());
                }
            }
        }
    }
}