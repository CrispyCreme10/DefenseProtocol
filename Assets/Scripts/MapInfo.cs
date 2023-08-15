using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Map Info")]
public class MapInfo : SerializedScriptableObject {
    [SerializeField] private string mapName;
    [SerializeField] private int rows = 1;
    [SerializeField] private int cols = 1;
    [TableMatrix(DrawElementMethod = "DrawCell", HorizontalTitle = "Tower Placement Cells", SquareCells = true)]
    [SerializeField]
    private bool[,] _mapTurretPlacementCells;

    public bool[,] MapTurretPlacementCells => _mapTurretPlacementCells;
    
    private static bool DrawCell(Rect rect, bool value) {
        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition)) {
            value = !value;
            GUI.changed = true;
            Event.current.Use();
        }
        
        EditorGUI.DrawRect(
            rect.Padding(1),
            value ? new Color(0.1f, 0.8f, 0.2f)
                : new Color(0.8f, 0.1f, 0.2f));

        return value;
    }

    // [OnInspectorInit]
    // private void CreateData() {
    //     Debug.Log($"{rows}, {cols}");
    //     _mapTurretPlacementCells = new bool[cols, rows];
    //     for (var x = 0; x < cols; x++) {
    //         for (var y = 0; y < rows; y++) {
    //             _mapTurretPlacementCells[x, y] = true;
    //         }
    //     }
    // }
}