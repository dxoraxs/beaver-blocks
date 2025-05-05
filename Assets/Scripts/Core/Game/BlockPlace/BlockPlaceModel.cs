using UniRx;

namespace BeaverBlocks.Core.Game.BlockPlace
{
    public class BlockPlaceModel
    {
        private readonly ReactiveProperty<string> _blockId = new(null);
        private readonly IntReactiveProperty _groupColor = new();

        public IReadOnlyReactiveProperty<string> BlockId => _blockId;
        public IReadOnlyReactiveProperty<int> GroupColor => _groupColor;

        public void SetBlock(string id, int groupColor)
        {
            _blockId.Value = id;
            _groupColor.Value = groupColor;
        }

        public void ClearPlace()
        {
            _blockId.Value = null;
        }
    }
}