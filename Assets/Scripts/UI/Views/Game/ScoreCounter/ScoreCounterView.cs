using TMPro;
using UniRx;
using UnityEngine;

namespace BeaverBlocks.UI.Views.Game.ScoreCounter
{
    public class ScoreCounterView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        private IScoreCounterPresenter _presenter;

        public void Initialize(IScoreCounterPresenter presenter)
        {
            _presenter = presenter;

            _presenter.ScoreStream.Subscribe(SetText).AddTo(this);
        }

        private void SetText(string score)
        {
            _text.text = score;
        }
    }
}