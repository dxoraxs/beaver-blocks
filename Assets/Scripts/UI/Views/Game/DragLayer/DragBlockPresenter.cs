using System;
using System.Threading;
using BeaverBlocks.Configs;
using BeaverBlocks.Configs.Data;
using BeaverBlocks.Core.Game;
using BeaverBlocks.UI.Views.Game.DragBlockObject;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;
using Object = UnityEngine.Object;

namespace BeaverBlocks.UI.Views.Game.DragLayer
{
    public class DragBlockPresenter : IDragBlockPresenter
    {
        private readonly IConfigsService _configsService;
        private readonly PrefabsConfig _prefabsConfig;
        private readonly GameSettings _gameSettings;
        private readonly DragBlockController _dragBlockController;
        private DragBlockObjectView _dragBlockObjectView;
        private Transform _dragObjectParent;

        [Preserve]
        public DragBlockPresenter(IConfigsService configsService, DragBlockController dragBlockController)
        {
            _configsService = configsService;
            _dragBlockController = dragBlockController;

            _prefabsConfig = _configsService.Get<PrefabsConfig>();
            _gameSettings = _configsService.Get<GameSettings>();

            _dragBlockController.OnStartMove += StartMove;
            _dragBlockController.OnMove += Move;
            _dragBlockController.OnEndMove += EndMove;
        }

        public void SetParent(Transform parent)
        {
            _dragObjectParent = parent;
        }

        private void StartMove(Vector2 startPosition, Sprite blockImage, Color color)
        {
            _dragBlockObjectView = SpawnNewView();
            var presenter = new DragBlockObjectPresenter(color, blockImage);
            _dragBlockObjectView.Initialize(presenter);
            _dragBlockObjectView.transform.position = startPosition;
        }

        private void Move(Vector2 position)
        {
            position.y += _gameSettings.DragVerticalOffset;
            _dragBlockObjectView.transform.position = Vector3.Lerp(_dragBlockObjectView.transform.position, position,
                _gameSettings.SpeedDragBlock * Time.deltaTime);
        }

        private void EndMove()
        {
            Object.Destroy(_dragBlockObjectView.gameObject);
        }

        private DragBlockObjectView SpawnNewView()
        {
            var view = Object.Instantiate(_prefabsConfig.DragBlockObjectView, _dragObjectParent);
            return view;
        }
    }
}