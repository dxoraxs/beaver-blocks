using System.Linq;
using BeaverBlocks.UI.Views.Game.BlockPlace;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace BeaverBlocks.UI.Views.Game.BottomView
{
    public class BottomBlocksView : MonoBehaviour
    {
        [SerializeField] private HorizontalLayoutGroup _horizontalLayoutGroup;
        [SerializeField] private Transform _content;
        private IBottomBlocksPresenter _presenter;

        public void Initialize(IBottomBlocksPresenter presenter)
        {
            _presenter = presenter;
            CreatePlaces();
        }

        private void CreatePlaces()
        {
            var placePresenters = _presenter.BottomPlacePresenters.ToArray();
            foreach (var placePresenter in placePresenters)
            {
                var newPlaceView = CreateNewPlaceView();
                newPlaceView.Initialize(placePresenter);
            }
        }
        
        
        private BlockPlaceView CreateNewPlaceView()
        {
            var newPlaceView = Instantiate(_presenter.PlaceViewPrefab, _content);
            return newPlaceView;
        }
    }
}