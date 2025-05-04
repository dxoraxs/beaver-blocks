using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using BeaverBlocks.Configs.Data;

[CustomEditor(typeof(BlockConfig))]
public class BlockConfigEditor : Editor
{
    private const int gridSize = 8;
    private const int cellSize = 25;
    private const int padding = 5;

    private HashSet<Vector2Int> selectedCells = new();

    private BlockConfig config;

    private void OnEnable()
    {
        config = (BlockConfig)target;
        
        LoadConfig();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Block Shape Editor", EditorStyles.boldLabel);

        Rect gridRect = GUILayoutUtility.GetRect(gridSize * (cellSize + padding), gridSize * (cellSize + padding));

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                Vector2Int cell = new(x, y);

                Rect cellRect = new(
                    gridRect.x + x * (cellSize + padding),
                    gridRect.y + y * (cellSize + padding),
                    cellSize,
                    cellSize
                );

                bool isSelected = selectedCells.Contains(cell);
                Color originalColor = GUI.color;
                GUI.color = isSelected ? Color.green : Color.white;
                if (GUI.Button(cellRect, ""))
                {
                    if (isSelected)
                        selectedCells.Remove(cell);
                    else
                        selectedCells.Add(cell);
                }
                GUI.color = originalColor;
            }
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Reload from config"))
        {
            LoadConfig();
        }
        if (GUILayout.Button("Save to config"))
        {
            Undo.RecordObject(config, "Update Shape");
            config.SetShape(new List<Vector2Int>(selectedCells).ToArray());
            EditorUtility.SetDirty(config);
        }
    }

    private void LoadConfig()
    {
        selectedCells = new HashSet<Vector2Int>(config.Shape);
    }
}
