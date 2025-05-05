using System.Collections.Generic;

namespace BeaverBlocks.Core.Game.BlockPlace
{
    public class BlockPlaceModelInstaller
    {
        private readonly uint _placeCount;
        private readonly Dictionary<uint, BlockPlaceModel> _placeModels = new();
        
        public IReadOnlyDictionary<uint, BlockPlaceModel> PlaceModels => _placeModels;

        public BlockPlaceModelInstaller(uint placeCount)
        {
            _placeCount = placeCount;
        }

        public void Initialize()
        {
            for (var index = 0u; index < _placeCount; index++)
            {
                _placeModels.Add(index, new BlockPlaceModel());
            }
        }
    }
}