using System;
using System.Threading;
using BeaverBlocks.UI.Views.Game.DragBlockObject;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting;
using Object = UnityEngine.Object;

namespace BeaverBlocks.Core.Game
{
    public class DragController : IDragController
    {
        public event Action<Vector2> OnUpdateDrag;
        
        private readonly IInputController _inputController;
        
        [Preserve]
        public DragController(IInputController inputController)
        {
            _inputController = inputController;
        }

        public async UniTask StartMove(CancellationTokenSource cancellationTokenSource)
        {
            await DragObject(cancellationTokenSource);
        }

        private async UniTask DragObject(CancellationTokenSource cancellationTokenSource)
        {
            try
            {
                while (true)
                {
                    await UniTask.DelayFrame(1, cancellationToken: cancellationTokenSource.Token);
                    OnUpdateDrag?.Invoke(_inputController.MousePosition);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}