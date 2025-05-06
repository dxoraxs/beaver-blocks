using BeaverBlocks.Configs;
using BeaverBlocks.Configs.Data;
using UniRx;
using UnityEngine.Scripting;

namespace BeaverBlocks.Core.Game
{
    public class ScoreManager : IScoreManager
    {
        private readonly IntReactiveProperty _score = new();
        private readonly IConfigsService _configsService;
        private readonly GameSettings _gameSettings;
        
        public IReadOnlyReactiveProperty<int> Score => _score;

        [Preserve]
        public ScoreManager(IConfigsService configsService)
        {
            _configsService = configsService;

            _gameSettings = _configsService.Get<GameSettings>();
        }

        public void AddBlockScore(int count)
        {
            _score.Value += (int)(count * _gameSettings.PointsPerCube);
        }
    }
}