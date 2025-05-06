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

        private void StartMove(DragBlockStartData data)
        {
            _dragBlockObjectView = SpawnNewView();
            var presenter = new DragBlockObjectPresenter(data.Color.DefaultColors, data.Sprite);
            _dragBlockObjectView.Initialize(presenter);
            _dragBlockObjectView.transform.position = data.StartPosition;
        }

        private void Move(DragBlockUpdateData dragUpdateData)
        {
            var position = dragUpdateData.Position;
            _dragBlockObjectView.transform.position = Vector3.Lerp(_dragBlockObjectView.transform.position, position,
                _gameSettings.SpeedDragBlock * Time.deltaTime);
        }

        private void EndMove(DragBlockResultData data)
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