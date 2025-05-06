using System.Linq;
using BeaverBlocks.UI.Views.Game.Cells;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BeaverBlocks.UI.Views.Game.GridCells
{
    public class GridCellsView : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup _gridLayoutGroup;
        [SerializeField] private Transform _content;
        private IGridCellsPresenter _presenter;
        
        private RectTransform RectTransform => transform as RectTransform;

        public void Initialize(IGridCellsPresenter presenter)
        {
            _presenter = presenter;

            SetGridSize();
            CreateCellViews();
            
            _presenter.SetRectTransform(RectTransform);
            _presenter.SetCellSize(_gridLayoutGroup.cellSize.x + _gridLayoutGroup.spacing.x);
        }

        private void SetGridSize()
        {
            var cellSize = RectTransform.rect.size.x / _presenter.SizeGrid - _gridLayoutGroup.spacing.x;
            _gridLayoutGroup.cellSize = Vector2.one * cellSize;
        }

        private void CreateCellViews()
        {
            var cellPresenters = _presenter.GetCellPresenters.ToArray();

            foreach (var cellPresenter in cellPresenters)
            {
                var newCellView = CreateNewCellView();
                newCellView.Initialize(cellPresenter);
            }
        }

        private CellView CreateNewCellView()
        {
            var newCellView = Instantiate(_presenter.GetCellViewPrefab, _content);
            return newCellView;
        }
    }
}