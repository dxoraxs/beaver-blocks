using System;
using System.Collections.Generic;
using BeaverBlocks.Configs;
using BeaverBlocks.Configs.Data;
using BeaverBlocks.Core.Cells;
using BeaverBlocks.UI.Views.Game.Cells;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;
using VContainer.Unity;

namespace BeaverBlocks.UI.Views.Game.GridCells
{
    public class GridCellPresenter : IGridCellsPresenter
    {
        private readonly IConfigsService _configsService;
        private readonly GameSettings _gameSettings;
        private readonly PrefabsConfig _prefabsConfig;
        private readonly CellPresentersManager _cellPresentersManager;
        private RectTransform _gridRectTransform;

        public CellView GetCellViewPrefab => _prefabsConfig.CellView;
        public uint SizeGrid => _gameSettings.GridSize;
        public IEnumerable<ICellPresenter> GetCellPresenters => _cellPresentersManager.GetCellPresenters;

        [Preserve]
        public GridCellPresenter(IConfigsService configsService, CellPresentersManager cellPresentersManager)
        {
            _configsService = configsService;
            _cellPresentersManager = cellPresentersManager;

            _gameSettings = _configsService.Get<GameSettings>();
            _prefabsConfig = _configsService.Get<PrefabsConfig>();
        }

        public void SetRectTransform(RectTransform rectTransform)
        {
            _gridRectTransform = rectTransform;
        }

        public bool IsPointInsideUIElement(Vector2 screenPoint)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(_gridRectTransform, screenPoint, null);
        }

        public (int x, int y) GetGridIndexFromScreenPoint(Vector2 point)
        {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_gridRectTransform, point, null,
                    out var localPoint))
                return (-1, -1);

            var rect = _gridRectTransform.rect;

            var xFromLeft = localPoint.x + rect.width / 2f;
            var yFromBottom = localPoint.y + rect.height / 2f;

            var cellWidth = rect.width / SizeGrid;
            var cellHeight = rect.height / SizeGrid;

            var xIndex = Mathf.FloorToInt(xFromLeft / cellWidth);
            var yIndex = Mathf.FloorToInt(yFromBottom / cellHeight);

            xIndex = Mathf.RoundToInt(Mathf.Clamp(xIndex, 0, SizeGrid));
            yIndex = (int)SizeGrid - 1 - Mathf.RoundToInt(Mathf.Clamp(yIndex, 0, SizeGrid));

            return (xIndex, yIndex);
        }
    }
}