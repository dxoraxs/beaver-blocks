using System.Collections.Generic;
using BeaverBlocks.UI.Views.Game.Cells;

namespace BeaverBlocks.Core.Cells
{
    public class CellModelInstaller
    {
        private readonly uint _sizeGrid;
        private readonly Dictionary<(int, int), CellModel> _cellModels = new(); 
        
        public IReadOnlyDictionary<(int, int), CellModel> CellModels => _cellModels;
        
        public CellModelInstaller(uint sizeGrid)
        {
            _sizeGrid = sizeGrid;
        }

        public void Initialize()
        {
            for (var x = 0; x < _sizeGrid; x++)
            {
                for (var y = 0; y < _sizeGrid; y++)
                {
                    _cellModels.Add((x, y), new CellModel());
                }
            }
        }
    }
}