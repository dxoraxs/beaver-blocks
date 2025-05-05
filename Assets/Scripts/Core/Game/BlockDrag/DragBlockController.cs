using System;
using System.Linq;
using System.Threading;
using BeaverBlocks.Configs;
using BeaverBlocks.Configs.Data;
using BeaverBlocks.Core.Game.BlockPlace;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Scripting;
using VContainer.Unity;

namespace BeaverBlocks.Core.Game
{
    public class DragBlockController
    {
        public event Action<Vector2, Sprite, Color> OnStartMove;
        public event Action<Vector2> OnMove;
        public event Action OnEndMove;
        private readonly IDragController _dragController;
        private readonly BlockPlacePresenterManager _blockPlacePresenterManager;
        private readonly IConfigsService _configsService;

        private readonly BlocksDatabase _blocksDatabase;
        private readonly CellColorsConfig _cellColorsConfig;

        [Preserve]
        public DragBlockController(IDragController dragController,
            BlockPlacePresenterManager blockPlacePresenterManager,
            IConfigsService configsService, IInputController inputController)
        {
            _dragController = dragController;
            _blockPlacePresenterManager = blockPlacePresenterManager;
            _configsService = configsService;

            _blocksDatabase = _configsService.Get<BlocksDatabase>();
            _cellColorsConfig = _configsService.Get<CellColorsConfig>();

            _dragController.OnUpdateDrag += position => OnMove?.Invoke(position);
        }

        public async UniTask StartDrag(Vector2 startPosition, string blockId, int groupIndex,
            CancellationTokenSource cancellationTokenSource)
        {
            var blockData = _blocksDatabase.BlockConfigs.First(block => block.Id == blockId);

            OnStartMove?.Invoke(startPosition, blockData.Sprite,
                _cellColorsConfig.CellColors[groupIndex].DefaultColors);
            
            await _dragController.StartMove(cancellationTokenSource);

            OnEndMove?.Invoke();
        }
    }
}