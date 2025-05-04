using System.Collections.Generic;
using BeaverBlocks.UI.Views.Game.BlockPlace;
using BeaverBlocks.UI.Views.Game.Cells;
using Unity.VisualScripting;
using UnityEngine.Scripting;

namespace BeaverBlocks.Core.Game.BlockPlace
{
    public class BlockPlaceManager
    {
        private readonly Dictionary<uint, IBlockPlacePresenter> _blockPlacePresenters = new();

        public IReadOnlyDictionary<uint, IBlockPlacePresenter> BlockPlacePresenters => _blockPlacePresenters;
        public IEnumerable<IBlockPlacePresenter> GetBlockPlacePresenters => _blockPlacePresenters.Values;

        [Preserve]
        public BlockPlaceManager()
        {
        }

        public void SetBlockPlaces(IReadOnlyDictionary<uint, IBlockPlacePresenter> blockPresenters)
        {
            _blockPlacePresenters.AddRange(blockPresenters);
        }
    }
}