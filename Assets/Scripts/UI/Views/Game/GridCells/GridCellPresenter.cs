﻿using System;
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
        private float _cellSize;

        public CellView GetCellViewPrefab => _prefabsConfig.CellView;
        public uint SizeGrid => _gameSettings.GridSize;
        public float SizeCell() => _cellSize;
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

        public void SetCellSize(float cellSize)
        {
            _cellSize = cellSize;
        }

        public bool IsPointInsideUIElement(Vector2 screenPoint)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(_gridRectTransform, screenPoint, null);
        }

        public (int x, int y) GetGridIndexFromScreenPoint(Vector2 point)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_gridRectTransform, point, null,
                out var localPoint);

            var rect = _gridRectTransform.rect;
            
            var xFromLeft = localPoint.x + rect.width / 2f;
            Debug.Log($"xFromLeft: {xFromLeft}, {xFromLeft / rect.width}");
            var yFromBottom = localPoint.y + rect.height / 2f;

            var cellWidth = rect.width / SizeGrid;

            var xIndex = Mathf.RoundToInt(xFromLeft / cellWidth);
            var yIndex = Mathf.RoundToInt(yFromBottom / cellWidth);

            //xIndex = (int)Mathf.Clamp(xIndex, -1, SizeGrid);
            yIndex = (int)SizeGrid - 1 - yIndex;

            return (xIndex, yIndex);
        }
    }
}