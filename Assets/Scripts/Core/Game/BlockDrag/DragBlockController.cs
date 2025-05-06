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

    public readonly struct DragBlockResultData
    {
        public readonly (int x, int y) IndexCell;

        public DragBlockResultData((int x, int y) indexCell)
        {
            IndexCell = indexCell;
        }
    }

    public class DragBlockController
    {
        public event Action<DragBlockStartData> OnStartMove;
        public event Action<DragBlockUpdateData> OnMove;
        public event Action<DragBlockResultData> OnEndMove;
        private readonly IDragController _dragController;
        private readonly IInputController _inputController;

        private readonly BlocksDatabase _blocksDatabase;
        private readonly CellColorsConfig _cellColorsConfig;
        private readonly GameSettings _gameSettings;

        private Func<float> _getGridCellSize;
        private Func<Vector2, (int, int)> _getCellIndex;
        private BlockConfig _blockConfig;

        [Preserve]
        public DragBlockController(IDragController dragController, IConfigsService configsService,
            IInputController inputController)
        {
            _dragController = dragController;
            _inputController = inputController;

            _blocksDatabase = configsService.Get<BlocksDatabase>();
            _cellColorsConfig = configsService.Get<CellColorsConfig>();
            _gameSettings = configsService.Get<GameSettings>();

            _dragController.OnUpdateDrag += OnUpdateDrag;
        }

        public void SetFuncGetCellIndex(Func<Vector2, (int, int)> getCellIndex, Func<float> getGridCellSize)
        {
            _getCellIndex = getCellIndex;
            _getGridCellSize = getGridCellSize;
        }

        public async UniTask StartDrag(Vector2 startPosition, string blockId, int groupIndex,
            CancellationTokenSource cancellationTokenSource)
        {
            _blockConfig = _blocksDatabase.BlockConfigs.First(block => block.Id == blockId);

            SendStartEvent(startPosition, blockId, groupIndex);

            await _dragController.StartMove(cancellationTokenSource);

            SendEndEvent();
        }

        private void SendStartEvent(Vector2 startPosition, string blockId, int groupIndex)
        {
            var startData = new DragBlockStartData(startPosition, _blockConfig.Sprite,
                _cellColorsConfig.CellColors[groupIndex], blockId, groupIndex);
            OnStartMove?.Invoke(startData);
        }

        private void SendEndEvent()
        {
            var position = CalculatePositionWithOffset(_inputController.MousePosition);
            var resultData = new DragBlockResultData(_getCellIndex(position));
            OnEndMove?.Invoke(resultData);
        }

        private void OnUpdateDrag(Vector2 position)
        {
            position = CalculatePositionWithOffset(position);
            var dragBlockUpdateData = new DragBlockUpdateData(position, _getCellIndex(position));
            OnMove?.Invoke(dragBlockUpdateData);
        }

        private Vector2 CalculatePositionWithOffset(Vector2 position)
        {
            position.y += _gameSettings.DragVerticalOffset;
            var gridCellScaleOffset = Vector2.Scale(Vector2.one * _getGridCellSize.Invoke(), _blockConfig.CenterOffset);
            gridCellScaleOffset.y *= -1;
            position -= gridCellScaleOffset;
            return position;
        }
    }
}