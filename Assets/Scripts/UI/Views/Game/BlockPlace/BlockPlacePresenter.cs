using UniRx;
using UnityEngine;

namespace BeaverBlocks.UI.Views.Game.BlockPlace
{
    public class BlockPlacePresenter : IBlockPlacePresenter
    {
        private readonly ReactiveProperty<Sprite> _blockSprite = new();
        
        public IReadOnlyReactiveProperty<Sprite> BlockIconStream => _blockSprite;

        public BlockPlacePresenter()
        {
            
        }
        
        public void OnPointerDown()
        {
            
        }
    }
}