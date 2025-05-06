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
            _dragBlockController = _iocFactory.Create<DragBlockController, IDragController>(_dragController);
            
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
                
                var blockConfig = _configsService.Get<BlocksDatabase>().BlockConfigs.First(block => block.Id == blockId);
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
                
                await OnMouseRelease(dragResult.Value, blockConfig, indexPlace, groupColor);
                void OnDragEnd(DragBlockResultData resultData)
                {
                    dragResult = resultData;
                }
            }

        }
        
        private async UniTask OnMouseRelease(DragBlockResultData resultData, BlockConfig blockConfig, uint indexPlace, int groupColor)
        {
            var newList = new (int, int)[blockConfig.Shape.Length];
            for (var i = 0; i < blockConfig.Shape.Length; i++)
            {
                newList[i] = (
                    resultData.IndexCell.x + blockConfig.Shape[i].x,
                    resultData.IndexCell.y + blockConfig.Shape[i].y
                );
            }

            if (!_cellModelManager.TryGetCellsEmpty(newList, out var cells))
            {
                _blockPlaceModelManager.SetPlace(indexPlace, blockConfig.Id, groupColor);
                return;
            }
            
            var cellModels = cells.ToArray();
            foreach (var model in cellModels)
            {
                model.SetBusy(groupColor);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

            var clearCells = _cellModelManager.TryGetAllCellsFromRowAndColumn(newList).ToArray();
            var delayTime = .5f / clearCells.Length;
            foreach (var cellModel in clearCells)
            {
                cellModel.ClearCell();
                await UniTask.Delay(TimeSpan.FromSeconds(delayTime));
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