using System;
using UniRx;
using UnityEngine;

namespace BeaverBlocks.UI.Views.Game.BlockPlace
{
    public class BlockPlacePresenter : IBlockPlacePresenter
    {
        private readonly ReactiveProperty<Sprite> _sprite = new();
        private readonly ReactiveProperty<Color> _color = new();
        private readonly Subject<Unit> _pointerDownStream = new();
        
        public IObservable<Unit> PointerDownStream => _pointerDownStream;
        public IReadOnlyReactiveProperty<Sprite> BlockIconStream => _sprite;
        public IReadOnlyReactiveProperty<Color> BlockColorStream => _color;
        
        public void OnPointerDown()
        {
            _pointerDownStream?.OnNext(Unit.Default);
        }

        public void SetSprite(Sprite sprite)
        {
            _sprite.Value = sprite;
        }

        public void SetColor(Color color)
        {
            _color.Value = color;
        }
    }
}