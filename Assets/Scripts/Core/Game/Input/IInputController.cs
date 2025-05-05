using System;
using UnityEngine;

namespace BeaverBlocks.Core.Game
{
    public interface IInputController
    {
        Vector2 MousePosition { get; }
        IObservable<bool> MouseDownStream { get; }
    }
}