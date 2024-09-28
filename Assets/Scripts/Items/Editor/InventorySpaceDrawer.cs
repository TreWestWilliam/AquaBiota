using Codice.Client.BaseCommands.BranchExplorer;
using System;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.VolumeComponent;

[CustomPropertyDrawer(typeof(InventorySpace))]

public class InventorySpaceDrawer : PropertyDrawer
{
    private static bool defaultValue = false;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);

        EditorGUI.BeginChangeCheck();
        int startingIndent = EditorGUI.indentLevel;
        EditorGUI.indentLevel += 2;

        SerializedProperty sizeProperty = property.FindPropertyRelative("Size");
        SerializedProperty occupiedProperty = property.FindPropertyRelative("occupiedSpots");

        Vector2Int initialSizeValue = sizeProperty.vector2IntValue;
        if(initialSizeValue.x < 1 || initialSizeValue.y < 1)
        {
            if(initialSizeValue.x < 1)
            {
                initialSizeValue.x = 1;
            }

            if(initialSizeValue.y < 1)
            {
                initialSizeValue.y = 1;
            }
            sizeProperty.vector2IntValue = initialSizeValue;
        }

        if(occupiedProperty.arraySize != (initialSizeValue.x * initialSizeValue.y))
        {
            occupiedProperty.arraySize = initialSizeValue.x * initialSizeValue.y;
        }

        bool[] occupiedValues = new bool[occupiedProperty.arraySize];
        for(int i = 0; i < occupiedProperty.arraySize; i++)
        {
            occupiedValues[i] = occupiedProperty.GetArrayElementAtIndex(i).boolValue;
        }

        bool[,] grid = lineToGrid(occupiedValues, initialSizeValue);

        Vector2Int adjustedSizeValue = EditorGUILayout.Vector2IntField("Size", initialSizeValue);

        defaultValue = EditorGUI.Toggle(position, "Default:", defaultValue);

        float toggleSize = 20f;

        EditorGUILayout.LabelField(" ", GUILayout.Height(2f));
        Rect gridRect = EditorGUILayout.BeginVertical();
        EditorGUI.DrawRect(new Rect(gridRect.x + 32f, gridRect.y - 1f, toggleSize * grid.GetLength(0) + 4f, toggleSize * grid.GetLength(1) + 2f), Color.grey);
        for(int y = 0; y < grid.GetLength(1); y++)
        {
            EditorGUILayout.LabelField(" ", GUILayout.Height(toggleSize - 2f));
            for(int x = 0; x < grid.GetLength(0); x++)
            {
                Rect place = new Rect(gridRect.x + 7 + (toggleSize * x), gridRect.y + (toggleSize * y), 45f, toggleSize);

                grid[x, y] = EditorGUI.Toggle(place, grid[x, y]);
                occupiedProperty.GetArrayElementAtIndex(coordToIndex(x, y, initialSizeValue)).boolValue = grid[x, y];
            }
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.LabelField(" ", GUILayout.Height(4f));

        if(adjustedSizeValue.x < 1 || adjustedSizeValue.y < 1)
        {
            if(adjustedSizeValue.x < 1)
            {
                adjustedSizeValue.x = 1;
            }

            if(adjustedSizeValue.y < 1)
            {
                adjustedSizeValue.y = 1;
            }
            sizeProperty.vector2IntValue = adjustedSizeValue;
        }

        if(adjustedSizeValue != initialSizeValue)
        {
            sizeProperty.vector2IntValue = adjustedSizeValue;
            grid = resizeGrid(grid, adjustedSizeValue);
            bool[] line = gridToLine(grid);
            occupiedProperty.arraySize = line.Length;
            for(int i = 0; i < line.Length; i++)
            {
                occupiedProperty.GetArrayElementAtIndex(i).boolValue = line[i];
            }
        }


        if(EditorGUI.EndChangeCheck())
        {
            property.serializedObject.ApplyModifiedProperties();
        }

        EditorGUI.indentLevel = startingIndent;
        EditorGUI.EndProperty();
    }

    private bool[,] lineToGrid(bool[] line, Vector2Int size)
    {
        bool[,] grid = new bool[size.x, size.y];

        int index = 0;
        for(int y = 0; y < size.y; y++)
        {
            for(int x = 0; x < size.x; x++)
            {
                if(index < line.Length)
                {
                    grid[x, y] = line[index];
                }
                else
                {
                    grid[x, y] = false;
                }
                index++;
            }
        }

        return grid;
    }

    private bool[] gridToLine(bool[,] grid)
    {
        int x = grid.GetLength(0);
        int y = grid.GetLength(1);
        bool[] line = new bool[x * y];

        for(int i = 0; i < line.Length; i++)
        {
            line[i] = grid[i % x, i / x];
        }

        return line;
    }

    private bool[,] resizeGrid(bool[,] grid, Vector2Int size)
    {
        bool[,] resizedGrid = new bool[size.x, size.y];
        for(int y = 0; y < size.y; y++)
        {
            for(int x = 0; x < size.x; x++)
            {
                if(x < grid.GetLength(0) && y < grid.GetLength(1))
                {
                    resizedGrid[x, y] = grid[x, y];
                }
                else
                {
                    resizedGrid[x, y] = defaultValue;
                }
            }
        }

        return resizedGrid;
    }

    private int coordToIndex(int x, int y, Vector2Int size)
    {
        return x + (y * size.x);
    }
}
