using System;
using System.Threading;
using BeaverBlocks.Core.Game;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Scripting;

namespace BeaverBlocks.UI.Views.Game.ScoreCounter
{
    public class ScoreCounterPresenter : IScoreCounterPresenter
    {
        private readonly StringReactiveProperty _scoreString = new();
        private readonly IScoreManager _scoreManager;
        private CancellationTokenSource _cancellationTokenSource;
        private int _lastScore;

        public IReadOnlyReactiveProperty<string> ScoreStream => _scoreString;

        [Preserve]
        public ScoreCounterPresenter(IScoreManager scoreManager)
        {
            _scoreManager = scoreManager;

            _scoreManager.Score.Subscribe(OnChangeScore);
        }

        private void OnChangeScore(int score)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            AnimateScore(score).Forget();
        }

        private async UniTask AnimateScore(int targetScore)
        {
            var startScore = _lastScore;

            var timer = 0f;
            try
            {
                while (timer < .5f)
                {
                    await UniTask.DelayFrame(1, cancellationToken: _cancellationTokenSource.Token);
                    timer += Time.deltaTime;
                    _lastScore = (int)Mathf.Lerp(startScore, targetScore, timer / 0.5f);
                    _scoreString.Value = _lastScore.ToString();
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}