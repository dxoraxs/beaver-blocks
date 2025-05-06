using UniRx;

namespace BeaverBlocks.UI.Views.Game.ScoreCounter
{
    public interface IScoreCounterPresenter
    {
        IReadOnlyReactiveProperty<string> ScoreStream { get; }
    }
}