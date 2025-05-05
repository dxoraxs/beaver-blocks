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
        private readonly BlockPlacePresenterManager _blockPlaceModelManager;

        public BlockPlaceView PlaceViewPrefab => _prefabsConfig.BlockPlaceView;
        public IEnumerable<IBlockPlacePresenter> BottomPlacePresenters => _blockPlaceModelManager.GetBlockPlacePresenters;

        public BottomBlocksPresenter(IConfigsService configsService, BlockPlacePresenterManager blockPlaceModelManager)
        {
            _configsService = configsService;
            _blockPlaceModelManager = blockPlaceModelManager;

            _prefabsConfig = _configsService.Get<PrefabsConfig>();
        }
    }
}