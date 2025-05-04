using System.Collections.Generic;
using BeaverBlocks.Configs;
using BeaverBlocks.Configs.Data;
using BeaverBlocks.Core.Game.BlockPlace;
using BeaverBlocks.UI.Views.Game.BlockPlace;

namespace BeaverBlocks.UI.Views.Game.BottomView
{
    public class BottomBlocksPresenter : IBottomBlocksPresenter
    {
        private readonly IConfigsService _configsService;
        private readonly PrefabsConfig _prefabsConfig;
        private readonly BlockPlaceManager _blockPlaceManager;

        public BlockPlaceView PlaceViewPrefab => _prefabsConfig.BlockPlaceView;
        public IEnumerable<IBlockPlacePresenter> BottomPlacePresenters => _blockPlaceManager.GetBlockPlacePresenters;

        public BottomBlocksPresenter(IConfigsService configsService, BlockPlaceManager blockPlaceManager)
        {
            _configsService = configsService;
            _blockPlaceManager = blockPlaceManager;

            _prefabsConfig = _configsService.Get<PrefabsConfig>();
        }
    }
}