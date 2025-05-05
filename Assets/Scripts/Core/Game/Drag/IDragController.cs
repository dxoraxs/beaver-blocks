using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BeaverBlocks.Core.Game
{
    public interface IDragController
    {
        event Action<Vector2> OnUpdateDrag;
        UniTask StartMove(CancellationTokenSource cancellationTokenSource);
    }
}