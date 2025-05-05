using System.Collections.Generic;
using BeaverBlocks.UI.Views.Game.Cells;
using UniRx;

namespace BeaverBlocks.Core.Cells
{
    public class CellPresentersInstaller
    {
        private readonly uint _count;
        private readonly List<ICellPresenter> _cellPresenters = new();

        public IEnumerable<ICellPresenter> CellPresenters => _cellPresenters;

        public CellPresentersInstaller(uint count)
        {
            _count = count;
        }

        public void Initialize()
        {
            for (var i = 0; i < _count * _count; i++)
            {
                _cellPresenters.Add(new CellPresenter());
            }
        }
    }
}