using System;
using UniRx;
using UnityEngine;
using UnityEngine.Scripting;
using VContainer.Unity;

namespace BeaverBlocks.Core.Game
{
    public class InputController : IInputController, ITickable
    {
        private readonly Subject<bool> _pressedInput = new();
        private IDisposable _subscription;
        
        public Vector2 MousePosition => Input.mousePosition;
        public IObservable<bool> MouseDownStream => _pressedInput;

        [Preserve]
        public InputController()
        {
        }

        public void Tick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _pressedInput.OnNext(true);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _pressedInput.OnNext(false);
            }
        }
    }
}