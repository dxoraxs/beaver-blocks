using System;
using System.Collections.Generic;
using BeaverBlocks.UI.Views;
using BeaverBlocks.UI.Views.Game;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace BeaverBlocks.UI
{
    public class PanelService : MonoBehaviour, IPanelService
    {
        [SerializeField] private GameView _gameView;

        private readonly Dictionary<Type, BaseView> _views = new();

        public void Initialize()
        {
            DontDestroyOnLoad(gameObject);
            
            AddToCache(_gameView);
        }

        public TView Get<TView>() where TView : BaseView
        {
            return (TView)_views[typeof(TView)];
        }

        private void AddToCache<TView>(TView view) where TView : BaseView
        {
            view.SetEnabled(false);
            _views.Add(view.GetType(), view);
        }
    }
}