﻿using System;
using System.Collections.Generic;
using BeaverBlocks.Configs.Data;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using VContainer.Unity;

namespace BeaverBlocks.Configs
{
    [CreateAssetMenu(fileName = "configs_service", menuName = "Create/Configs " + nameof(ConfigsService), order = 0)]
    public class ConfigsService : ScriptableObject, IConfigsService, IInitializable
    {
        [SerializeField] private LevelsDatabase _levelsDatabase;
        [SerializeField] private BlocksDatabase _blocksDatabase;
        [SerializeField] private GameSettings _gameSettings;
        [SerializeField] private PrefabsConfig _prefabsConfig;
        [SerializeField] private CellColorsConfig _cellColorsConfig;
        private readonly Dictionary<Type, ScriptableObject> _configs = new();

        public void Initialize()
        {
            AddToCache(_levelsDatabase);
            AddToCache(_blocksDatabase);
            AddToCache(_gameSettings);
            AddToCache(_prefabsConfig);
            AddToCache(_cellColorsConfig);
        }

        public T Get<T>() where T : ScriptableObject
        {
            return (T)_configs[typeof(T)];
        }
        
        private void AddToCache<T>(T config) where T : ScriptableObject
        {
            _configs.Add(config.GetType(), config);
        }
    }
}