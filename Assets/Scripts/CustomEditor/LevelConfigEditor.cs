using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using BeaverBlocks.Configs.Data;
using Random = UnityEngine.Random;

[CustomEditor(typeof(LevelConfig))]
public class LevelConfigEditor : Editor
{
    private LevelConfig config;
    private uint gridSize = 8;
    private Dictionary<Vector2Int, int> placedCells = new();

    private int selectedGroupIndex = 0;
    private bool editMode = true;

    private void OnEnable()
    {
        config = (LevelConfig)target;
        LoadGridSizeFromGameSettings();

        placedCells.Clear();
        if (config.PrePlacedCells != null)
        {
            foreach (var cell in config.PrePlacedCells)
            {
                placedCells[cell.Position] = cell.GroupIndex;
            }
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space(10);

        editMode = EditorGUILayout.Toggle("Edit Mode", editMode);

        using (new EditorGUI.DisabledScope(!editMode))
        {
            selectedGroupIndex = EditorGUILayout.IntField("Selected Group Index", selectedGroupIndex);
        }

        var cellSize = 25f;
        var padding = 5f;
        var gridRect = GUILayoutUtility.GetRect(gridSize * (cellSize + padding), gridSize * (cellSize + padding));

        for (var y = 0; y < gridSize; y++)
        {
            for (var x = 0; x < gridSize; x++)
            {
                Vector2Int cell = new(x, y);
                Rect cellRect = new(
                    gridRect.x + x * (cellSize + padding),
                    gridRect.y + y * (cellSize + padding),
                    cellSize,
                    cellSize
                );

                var inGroup = placedCells.TryGetValue(cell, out var group) && group == selectedGroupIndex;
                var hasAnyGroup = placedCells.TryGetValue(cell, out var cellGroup);

                GUI.color = editMode
                    ? (inGroup
                        ? GetColorForGroup(selectedGroupIndex)
                        : hasAnyGroup
                            ? new Color(0.7f, 0.7f, 0.7f)
                            : Color.white)
                    : (hasAnyGroup
                        ? GetColorForGroup(cellGroup)
                        : Color.white);

                if (GUI.Button(cellRect, hasAnyGroup ? (editMode && cellGroup != selectedGroupIndex ? cellGroup.ToString() : "") : ""))
                {
                    if (!editMode) continue;

                    if (inGroup)
                        placedCells.Remove(cell);
                    else
                        placedCells[cell] = selectedGroupIndex;
                }

                GUI.color = Color.white;
            }
        }

        EditorGUILayout.Space(10);

        using (new EditorGUI.DisabledScope(!editMode))
        {
            if (GUILayout.Button("Save PrePlacedCells"))
            {
                Undo.RecordObject(config, "Update PrePlacedCells");
                var newCells = new List<PrePlacedCell>();
                foreach (var kv in placedCells)
                    newCells.Add(new PrePlacedCell(kv.Key, kv.Value));
                config.SetCells(newCells.ToArray());
                EditorUtility.SetDirty(config);
            }
        }
        
        if (GUILayout.Button("Clear All"))
        {
            config.SetCells(Array.Empty<PrePlacedCell>());
            placedCells.Clear();
            EditorUtility.SetDirty(config);
        }
    }

    private void LoadGridSizeFromGameSettings()
    {
        var guids = AssetDatabase.FindAssets("t:GameSettings", new[] { "Assets/Configs" });
        if (guids.Length == 0) return;

        var settingsPath = AssetDatabase.GUIDToAssetPath(guids[0]);
        var settings = AssetDatabase.LoadAssetAtPath<GameSettings>(settingsPath);
        if (settings != null)
            gridSize = settings.GridSize;
    }

    private Color GetColorForGroup(int groupIndex)
    {
        Random.InitState(groupIndex);
        return Color.HSVToRGB((groupIndex * 0.123f) % 1f, 0.6f, 1f);
    }
}
