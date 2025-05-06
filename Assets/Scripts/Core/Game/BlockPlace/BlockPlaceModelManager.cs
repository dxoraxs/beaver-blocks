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

        public void ClearAll()
        {
            foreach (var placeModel in _placeModels.Values)
            {
                placeModel.ClearPlace();
            }
        }

        public void SetPlace(uint index, string id, int groupColor)
        {
            PlaceModels[index].SetBlock(id, groupColor);
        }

        public void ClearPlace(uint index)
        {
            PlaceModels[index].ClearPlace();
        }

        public void SetModels(IReadOnlyDictionary<uint, BlockPlaceModel> placeModels)
        {
            _placeModels.AddRange(placeModels);
        }
    }
}