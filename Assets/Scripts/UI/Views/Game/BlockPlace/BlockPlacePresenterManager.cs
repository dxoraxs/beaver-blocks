using System;
using System.Collections.Generic;
using System.Linq;
using BeaverBlocks.Configs;
using BeaverBlocks.Configs.Data;
using BeaverBlocks.UI.Views.Game.BlockPlace;
using BeaverBlocks.UI.Views.Game.Cells;
using UniRx;
using Unity.VisualScripting;
using UnityEngine.Scripting;
using Unit = UniRx.Unit;

namespace BeaverBlocks.Core.Game.BlockPlace
{
    public class BlockPlacePresenterManager
    {
        private readonly Subject<uint> _pointerDownStream = new();
        private readonly IConfigsService _configsService;
        private readonly Dictionary<uint, IBlockPlacePresenter> _blockPlacePresenters = new();

        public IObservable<uint> PointerDownStream => _pointerDownStream;
        public IReadOnlyDictionary<uint, IBlockPlacePresenter> BlockPlacePresenters => _blockPlacePresenters;
        public IEnumerable<IBlockPlacePresenter> GetBlockPlacePresenters => _blockPlacePresenters.Values;

        [Preserve]
        public BlockPlacePresenterManager(IConfigsService configsService)
        {
            _configsService = configsService;
        }

        public void SetBlockPlaces(IReadOnlyDictionary<uint, IBlockPlacePresenter> blockPresenters,
            IEnumerable<BlockPlaceModel> blockPlaceModels)
        {
            _blockPlacePresenters.AddRange(blockPresenters);
            var modelArray = blockPlaceModels.ToArray();
            for (var index = 0u; index < modelArray.Length && index < blockPlaceModels.Count(); index++)
            {
                var model = modelArray[index];
                var presenter = _blockPlacePresenters[index];
                var placeIndex = index;
                presenter.PointerDownStream.Subscribe(_ => OnBlockPlaceDown(placeIndex));
                model.BlockId.Subscribe(value => OnChangeSpriteModel(presenter, value));
                model.GroupColor.Subscribe(value => OnChangeColorModel(presenter, value));
            }
        }

        private void OnBlockPlaceDown(uint index)
        {
            _pointerDownStream.OnNext(index);
        }

        private void OnChangeColorModel(IBlockPlacePresenter presenter, int value)
        {
            presenter.SetColor(_configsService.Get<CellColorsConfig>().CellColors[value].DefaultColors);
        }

        private void OnChangeSpriteModel(IBlockPlacePresenter presenter, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                presenter.SetSprite(null);
            }
            else
            {
                presenter.SetSprite(_configsService.Get<BlocksDatabase>().BlockConfigs
                    .First(block => block.Id == value).Sprite);
            }
        }
    }
}