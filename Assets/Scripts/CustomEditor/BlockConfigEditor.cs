﻿using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using BeaverBlocks.Configs.Data;
using Unity.VisualScripting;

#if UNITY_EDITOR
[CustomEditor(typeof(BlockConfig))]
public class BlockConfigEditor : Editor
{
    private const int _cellSize = 25;
    private const int _padding = 5;

    private readonly HashSet<Vector2Int> _selectedCells = new();

    private uint _gridSize = 8;
    private BlockConfig _config;

    private void OnEnable()
    {
        _config = (BlockConfig)target;

        LoadConfig();
        TryLoadGridSizeFromGameSettings();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Block Shape Editor", EditorStyles.boldLabel);

        var gridRect =
            GUILayoutUtility.GetRect(_gridSize * (_cellSize + _padding), _gridSize * (_cellSize + _padding));

        for (var x = 0; x < _gridSize; x++)
        {
            for (var y = 0; y < _gridSize; y++)
            {
                Vector2Int cell = new(x, y);

                Rect cellRect = new(
                    gridRect.x + x * (_cellSize + _padding),
                    gridRect.y + (_gridSize - y - 1) * (_cellSize + _padding),
                    _cellSize,
                    _cellSize
                );

                var isSelected = _selectedCells.Contains(cell);
                var originalColor = GUI.color;
                GUI.color = isSelected ? Color.green : Color.white;
                if (GUI.Button(cellRect, ""))
                {
                    if (isSelected)
                        _selectedCells.Remove(cell);
                    else
                        _selectedCells.Add(cell);
                }

                GUI.color = originalColor;
            }
        }

        var offsetToDownGrid = Vector2.down * (_gridSize * (_cellSize + _padding));
        DrawCenterCross(gridRect.position - offsetToDownGrid, _cellSize, _padding, _config.CenterOffset);

        GUILayout.Space(10);

        if (GUILayout.Button("Reload from config"))
        {
            LoadConfig();
        }

        if (GUILayout.Button("Save to config"))
        {
            Undo.RecordObject(_config, "Update Shape");
            _config.SetShape(new List<Vector2Int>(_selectedCells).ToArray());
            EditorUtility.SetDirty(_config);
        }
    }

    private void DrawCenterCross(Vector2 gridOrigin, float cellSize, float padding, Vector2 centerOffset)
    {
        Handles.color = Color.red;

        var cellFullSize = new Vector2(cellSize + padding, cellSize + padding);
        var scaleOffset = Vector2.Scale(centerOffset, cellFullSize);
        scaleOffset.y *= -1;
        var centerPos = gridOrigin + scaleOffset;

        var half = cellSize * 0.4f;
        var horStart = centerPos + new Vector2(-half, 0);
        var horEnd = centerPos + new Vector2(+half, 0);
        var vertStart = centerPos + new Vector2(0, -half);
        var vertEnd = centerPos + new Vector2(0, +half);

        Handles.DrawLine(horStart, horEnd);
        Handles.DrawLine(vertStart, vertEnd);
    }

    private void TryLoadGridSizeFromGameSettings()
    {
        var guids = AssetDatabase.FindAssets("t:GameSettings", new[] { "Assets/Configs" });

        if (guids.Length == 0) return;

        var settingsPath = AssetDatabase.GUIDToAssetPath(guids[0]);
        var settings = AssetDatabase.LoadAssetAtPath<GameSettings>(settingsPath);

        if (settings != null)
        {
            _gridSize = settings.GridSize;
        }
    }

    private void LoadConfig()
    {
        _selectedCells.Clear();
        _selectedCells.AddRange(_config.Shape);
    }
}
#endif