using System;
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
            IIocFactory iocFactory, IInputController inputController)
        {
            _iocFactory = iocFactory;
            _inputController = inputController;
            _configsService = configsService;
            _panelService = panelService;

            _levelsDatabase = _configsService.Get<LevelsDatabase>();
            _gameSettings = _configsService.Get<GameSettings>();

            _dragController = _iocFactory.Create<DragController>();
            _cellModelManager = _iocFactory.Create<CellModelManager>();
            _cellPresentersManager = _iocFactory.Create<CellPresentersManager>();
            _blockPlaceModelManager = _iocFactory.Create<BlockPlaceModelManager>();
            _blockPlacePresenterManager = _iocFactory.Create<BlockPlacePresenterManager>();
            _dragBlockController = _iocFactory
                .Create<DragBlockController, IDragController, BlockPlacePresenterManager>(
                    _dragController, _blockPlacePresenterManager);

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

            var cellLevelInstaller = _iocFactory.Create<CellLevelInstaller, CellModelManager>(_cellModelManager);
            cellLevelInstaller.Install(currentLevel);

            await UniTask.Delay(TimeSpan.FromSeconds(1));

            var initialLevelBlockPlaceInstaller =
                _iocFactory.Create<InitialLevelBlockInstaller, BlockPlaceModelManager>(_blockPlaceModelManager);
            initialLevelBlockPlaceInstaller.Install(currentLevel);

            while (true)
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

                var groupColor = placeModel.GroupColor.Value;
                _blockPlaceModelManager.ClearPlace(indexPlace);
                var cancelTokenSource = new CancellationTokenSource();
                _dragBlockController.StartDrag(_inputController.MousePosition, blockId,
                    groupColor, cancelTokenSource).Forget();

                await _inputController.MouseDownStream.Where(value => !value).First().ToUniTask();
                cancelTokenSource.Cancel();
                dragBlockPreviewController.Dispose();

                var verticalOffset = Vector2.up * _gameSettings.DragVerticalOffset;
                var targetPoint = _inputController.MousePosition + verticalOffset;
                if (!_gamePresenter.IsGridRaycast(targetPoint))
                {
                    _blockPlaceModelManager.SetPlace(indexPlace, blockId, groupColor);
                    continue;
                }

                var nearestCellIndex = _gamePresenter.GetGridIndexFromScreenPoint(targetPoint);
                if (_cellModelManager.IsCellBusy(nearestCellIndex))
                {
                    _blockPlaceModelManager.SetPlace(indexPlace, blockId, groupColor);
                    continue;
                }

                _cellModelManager.CellModels[nearestCellIndex].SetPreview(groupColor);
            }
        }

        private LevelConfig GetLevel()
        {
            var allLevel = _levelsDatabase.LevelConfigs.ToArray();
            var currentLevel = allLevel[_levelIndex % allLevel.Length];
            return currentLevel;
        }
    }
}