﻿using System;
using System.Linq;
using System.Threading;
using BeaverBlocks.Configs;
using BeaverBlocks.Configs.Data;
using BeaverBlocks.Core.Cells;
using BeaverBlocks.Core.Game.BlockPlace;
using BeaverBlocks.Core.Game.Level;
using BeaverBlocks.DI.Factories;
using BeaverBlocks.UI;
using BeaverBlocks.UI.Views.Game;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Scripting;

namespace BeaverBlocks.Core.Game
{
    public class GameplayModeController
    {
        private readonly IIocFactory _iocFactory;
        private readonly IConfigsService _configsService;
        private readonly IPanelService _panelService;
        private readonly IInputController _inputController;
        private readonly IScoreManager _scoreManager;

        private readonly LevelsDatabase _levelsDatabase;
        private readonly GameSettings _gameSettings;
        private readonly CellModelManager _cellModelManager;
        private readonly CellPresentersManager _cellPresentersManager;
        private readonly BlockPlaceModelManager _blockPlaceModelManager;
        private readonly BlockPlacePresenterManager _blockPlacePresenterManager;
        private readonly IDragController _dragController;
        private DragBlockController _dragBlockController;
        private uint _levelIndex;
        private IGamePresenter _gamePresenter;

        private GameView GameView => _panelService.Get<GameView>();

        [Preserve]
        public GameplayModeController(IConfigsService configsService, IPanelService panelService,
            IIocFactory iocFactory, IInputController inputController, IScoreManager scoreManager)
        {
            _iocFactory = iocFactory;
            _inputController = inputController;
            _scoreManager = scoreManager;
            _configsService = configsService;
            _panelService = panelService;

            _levelsDatabase = _configsService.Get<LevelsDatabase>();
            _gameSettings = _configsService.Get<GameSettings>();

            _dragController = _iocFactory.Create<DragController>();
            _cellModelManager = _iocFactory.Create<CellModelManager>();
            _cellPresentersManager = _iocFactory.Create<CellPresentersManager>();
            _blockPlaceModelManager = _iocFactory.Create<BlockPlaceModelManager>();
            _blockPlacePresenterManager = _iocFactory.Create<BlockPlacePresenterManager>();
            _dragBlockController =
                _iocFactory.Create<DragBlockController, IDragController, CellModelManager>(_dragController,
                    _cellModelManager);

            Initialize();
        }

        private void Initialize()
        {
            InitializeCells();
            InitializeBlockPlaces();

            _gamePresenter = _iocFactory
                .Create<GamePresenter, CellPresentersManager, BlockPlacePresenterManager, DragBlockController>(
                    _cellPresentersManager, _blockPlacePresenterManager, _dragBlockController);
            GameView.Initialize(_gamePresenter);
            GameView.SetEnabled(true);

            StartLevel().Forget();
        }

        private void InitializeCells()
        {
            var cellModelInstaller = new CellModelInstaller(_configsService.Get<GameSettings>().GridSize);
            cellModelInstaller.Initialize();
            _cellModelManager.SetModels(cellModelInstaller.CellModels);

            var cellPresentersInstaller = new CellPresentersInstaller(_configsService.Get<GameSettings>().GridSize);
            cellPresentersInstaller.Initialize();
            _cellPresentersManager.SetCells(cellPresentersInstaller.CellPresenters,
                _cellModelManager.CellModels.Values);
        }

        private void InitializeBlockPlaces()
        {
            var blockPlaceModelInstaller =
                new BlockPlaceModelInstaller(_configsService.Get<GameSettings>().CountBottomPlace);
            blockPlaceModelInstaller.Initialize();
            _blockPlaceModelManager.SetModels(blockPlaceModelInstaller.PlaceModels);

            var blockPlacePresentorInstaller =
                new BlockPlacePresenterInstaller(_configsService.Get<GameSettings>().CountBottomPlace);
            blockPlacePresentorInstaller.Initialize();
            _blockPlacePresenterManager.SetBlockPlaces(blockPlacePresentorInstaller.BlockPlacePresenters,
                blockPlaceModelInstaller.PlaceModels.Values);
        }

        private async UniTask StartLevel()
        {
            var currentLevel = GetLevel();

            var countOfUsedBlock = currentLevel.CountMaxBlock;

            var cellLevelInstaller = _iocFactory.Create<CellLevelInstaller, CellModelManager>(_cellModelManager);
            cellLevelInstaller.Install(currentLevel);

            await UniTask.Delay(TimeSpan.FromSeconds(1));

            var initialLevelBlockPlaceInstaller =
                _iocFactory.Create<InitialLevelBlockInstaller, BlockPlaceModelManager>(_blockPlaceModelManager);
            initialLevelBlockPlaceInstaller.Install(currentLevel);

            do
            {
                var dragBlockPreviewController =
                    _iocFactory.Create<DragBlockPreviewController, CellModelManager, DragBlockController>(
                        _cellModelManager, _dragBlockController);
                var indexPlace = await _blockPlacePresenterManager.PointerDownStream.First().ToUniTask();

                var placeModel = _blockPlaceModelManager.PlaceModels[indexPlace];
                var blockId = placeModel.BlockId.Value;
                if (string.IsNullOrEmpty(blockId))
                {
                    continue;
                }

                var blockConfig = _configsService.Get<BlocksDatabase>().BlockConfigs
                    .First(block => block.Id == blockId);
                var groupColor = placeModel.GroupColor.Value;
                _blockPlaceModelManager.ClearPlace(indexPlace);
                var cancelTokenSource = new CancellationTokenSource();
                _dragBlockController.StartDrag(_inputController.MousePosition, blockId,
                    groupColor, cancelTokenSource).Forget();

                DragBlockResultData? dragResult = null;
                _dragBlockController.OnEndMove += OnDragEnd;

                await _inputController.MouseDownStream.Where(value => !value).First().ToUniTask();
                cancelTokenSource.Cancel();
                dragBlockPreviewController.Dispose();

                await UniTask.WaitUntil(() => dragResult.HasValue);
                var setBlockResult = await OnMouseRelease(dragResult.Value, blockConfig, indexPlace, groupColor);
                if (setBlockResult)
                {
                    countOfUsedBlock--;
                }

                void OnDragEnd(DragBlockResultData resultData)
                {
                    dragResult = resultData;
                }
            } while (_cellModelManager.CellBusyCounter > 0 && countOfUsedBlock > 0);

            if (_cellModelManager.CellBusyCounter == 0)
            {
                _levelIndex++;
            }

            _cellModelManager.ClearAll();
            _blockPlaceModelManager.ClearAll();

            await UniTask.Delay(TimeSpan.FromSeconds(1));

            StartLevel().Forget();
        }

        private async UniTask<bool> OnMouseRelease(DragBlockResultData resultData, BlockConfig blockConfig,
            uint indexPlace,
            int groupColor)
        {
            var newList = resultData.IndexCell;

            if (!_cellModelManager.TryGetCellsEmpty(newList))
            {
                _blockPlaceModelManager.SetPlace(indexPlace, blockConfig.Id, groupColor);

                await UniTask.Delay(TimeSpan.FromSeconds(1f));
                return false;
            }

            foreach (var cellKey in newList)
            {
                _cellModelManager.SetBusy(cellKey, groupColor);
            }

            var clearCells = _cellModelManager.GetAllCellsFromRowAndColumn(newList).ToArray();
            foreach (var clearCell in clearCells)
            {
                _cellModelManager.SetColor(clearCell, groupColor);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            var delayTime = .5f / clearCells.Length;
            _scoreManager.AddBlockScore(clearCells.Length);
            foreach (var cellKey in clearCells)
            {
                _cellModelManager.ClearCell(cellKey);
                await UniTask.Delay(TimeSpan.FromSeconds(delayTime));
            }

            return true;
        }

        private LevelConfig GetLevel()
        {
            var allLevel = _levelsDatabase.LevelConfigs.ToArray();
            var currentLevel = allLevel[_levelIndex % allLevel.Length];
            return currentLevel;
        }
    }
}