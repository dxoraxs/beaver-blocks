using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Scripting;

namespace BeaverBlocks.Core.Game.BlockPlace
{
    public class BlockPlaceModelManager
    {
        private readonly Dictionary<uint, BlockPlaceModel> _placeModels = new();

        public IReadOnlyDictionary<uint, BlockPlaceModel> PlaceModels => _placeModels;

        [Preserve]
        public BlockPlaceModelManager()
        {
        }

        public void SetModels(IReadOnlyDictionary<uint, BlockPlaceModel> placeModels)
        {
            _placeModels.AddRange(placeModels);
        }
    }
}