using System;
using UniRx;
using UnityEngine;

namespace BeaverBlocks.UI.Views.Game.BlockPlace
{
    public class BlockPlacePresenter : IBlockPlacePresenter
    {
        private readonly ReactiveProperty<Sprite> _sprite = new();
        private readonly ReactiveProperty<Color> _color = new();

        public event Action OnBlockPointDown;
        public IReadOnlyReactiveProperty<Sprite> BlockIconStream => _sprite;
        public IReadOnlyReactiveProperty<Color> BlockColorStream => _color;

        public BlockPlacePresenter()
        {
            
        }
        
        public void OnPointerDown()
        {
            OnBlockPointDown?.Invoke();
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