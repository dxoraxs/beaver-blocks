using System;
using System.Threading;
using BeaverBlocks.Configs;
using BeaverBlocks.Configs.Data;
using BeaverBlocks.Core.Game;
using BeaverBlocks.UI.Views.Game.DragBlockObject;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting;
using Object = UnityEngine.Object;

namespace BeaverBlocks.UI.Views.Game.DragLayer
{
    public class DragBlockPresenter : IDragBlockPresenter
    {
        private readonly IConfigsService _configsService;
        private readonly IInputController _inputController;
        private readonly CellColorsConfig _cellColorsConfig;
        private readonly PrefabsConfig _prefabsConfig;
        private readonly GameSettings _gameSettings;
        private CancellationTokenSource _cancellationTokenSource;
        private Transform _dragObjectParent;

        [Preserve]
        public DragBlockPresenter(IConfigsService configsService, IInputController inputController)
        {
            _configsService = configsService;
            _inputController = inputController;

            _cellColorsConfig = _configsService.Get<CellColorsConfig>();
            _prefabsConfig = _configsService.Get<PrefabsConfig>();
            _gameSettings = _configsService.Get<GameSettings>();
        }

        public void SetParent(Transform parent)
        {
            _dragObjectParent = parent;
        }

        public void StartMove(Vector2 startPosition, string blockId, int groupColor)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var newDragObject = SpawnNewView();
            newDragObject.transform.position = startPosition;
            var dragObjectPresenter =
                new DragBlockObjectPresenter(_cellColorsConfig.CellColors[groupColor].DefaultColors);
            newDragObject.Initialize(dragObjectPresenter);

            DragObject(newDragObject.transform).Forget();
        }

        private async UniTask DragObject(Transform dragObject)
        {
            try
            {
                while (true)
                {
                    await UniTask.DelayFrame(1, cancellationToken: _cancellationTokenSource.Token);
                    var newPoint = _inputController.MousePosition + Vector2.up * _gameSettings.DragVerticalOffset;
                    dragObject.position = Vector3.Lerp(dragObject.position, newPoint, .5f);
                }
            }
            catch (OperationCanceledException)
            {
            }

            Object.Destroy(dragObject.gameObject);
        }

        public void StopMove()
        {
            _cancellationTokenSource.Cancel();
        }

        private DragBlockObjectView SpawnNewView()
        {
            var view = Object.Instantiate(_prefabsConfig.DragBlockObjectView, _dragObjectParent);
            return view;
        }
    }
}