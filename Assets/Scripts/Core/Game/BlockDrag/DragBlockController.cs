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
    public readonly struct DragBlockStartData
    {
        public readonly Vector2 StartPosition;
        public readonly Sprite Sprite;
        public readonly PairColorValue Color;
        public readonly int ColorGroup;
        public readonly string BlockId;

        public DragBlockStartData(Vector2 startPosition, Sprite sprite, PairColorValue color, string blockId,
            int colorGroup)
        {
            StartPosition = startPosition;
            Sprite = sprite;
            Color = color;
            BlockId = blockId;
            ColorGroup = colorGroup;
        }
    }

    public readonly struct DragBlockUpdateData
    {
        public readonly Vector2 Position;
        public readonly (int x, int y) IndexCell;

        public DragBlockUpdateData(Vector2 position, (int, int) indexCell)
        {
            Position = position;
            IndexCell = indexCell;
        }
    }

    public class DragBlockController
    {
        public event Action<DragBlockStartData> OnStartMove;
        public event Action<DragBlockUpdateData> OnMove;
        public event Action OnEndMove;
        private readonly IDragController _dragController;

        private readonly BlocksDatabase _blocksDatabase;
        private readonly CellColorsConfig _cellColorsConfig;
        private readonly GameSettings _gameSettings;

        private Func<Vector2, (int, int)> _getCellIndex;

        [Preserve]
        public DragBlockController(IDragController dragController, IConfigsService configsService,
            IInputController inputController)
        {
            _dragController = dragController;

            _blocksDatabase = configsService.Get<BlocksDatabase>();
            _cellColorsConfig = configsService.Get<CellColorsConfig>();
            _gameSettings = configsService.Get<GameSettings>();

            _dragController.OnUpdateDrag += OnUpdateDrag;
        }

        public void SetFuncGetCellIndex(Func<Vector2, (int, int)> getCellIndex)
        {
            _getCellIndex = getCellIndex;
        }

        public async UniTask StartDrag(Vector2 startPosition, string blockId, int groupIndex,
            CancellationTokenSource cancellationTokenSource)
        {
            var blockData = _blocksDatabase.BlockConfigs.First(block => block.Id == blockId);

            var startData = new DragBlockStartData(startPosition, blockData.Sprite,
                _cellColorsConfig.CellColors[groupIndex], blockId, groupIndex);
            OnStartMove?.Invoke(startData);

            await _dragController.StartMove(cancellationTokenSource);

            OnEndMove?.Invoke();
        }

        private void OnUpdateDrag(Vector2 position)
        {
            position.y += _gameSettings.DragVerticalOffset;
            var dragBlockUpdateData = new DragBlockUpdateData(position, _getCellIndex(position));
            OnMove?.Invoke(dragBlockUpdateData);
        }
    }
}