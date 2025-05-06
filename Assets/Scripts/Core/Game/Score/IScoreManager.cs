using UniRx;

namespace BeaverBlocks.Core.Game
{
    public interface IScoreManager
    {
        IReadOnlyReactiveProperty<int> Score { get; }
        void AddBlockScore(int count);
    }
}