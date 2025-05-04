using System.Collections.Generic;
using BeaverBlocks.UI.Views.Game.BlockPlace;

namespace BeaverBlocks.Core.Game.BlockPlace
{
    public class BlockPlaceInstaller
    {
        private readonly uint _countOfPlaces;
        private readonly Dictionary<uint, IBlockPlacePresenter> _blockPlacePresenters = new();

        public IReadOnlyDictionary<uint, IBlockPlacePresenter> BlockPlacePresenters => _blockPlacePresenters;

        public BlockPlaceInstaller(uint countOfPlaces)
        {
            _countOfPlaces = countOfPlaces;
        }

        public void Initialize()
        {
            for (var i = 0u; i < _countOfPlaces; i++)
            {
                _blockPlacePresenters.Add(i, new BlockPlacePresenter());
            }
        }
    }
}