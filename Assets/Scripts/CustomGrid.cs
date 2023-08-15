using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

public class CustomGrid : SerializedMonoBehaviour {
    [SerializeField] private MapInfo mapInfo;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private int rows = 0;
    [SerializeField] private int cols = 0;
    [SerializeField] private float xOffset = 0f;
    [SerializeField] private float yOffset = 0f;
    [SerializeField] private float wireCubeSize = 1f;

    private Vector3 GetNearestPointOnGrid(Vector3 position) {
        // position -= transform.position;

        var xCount = Mathf.RoundToInt(position.x / cellSize) + xOffset;
        var yCount = Mathf.RoundToInt(position.y / cellSize) + yOffset;
        var zCount = Mathf.RoundToInt(position.z / cellSize);

        var result = new Vector3(xCount * cellSize, yCount * cellSize, zCount * cellSize);
        // result += transform.position;
        return result;
    }

    private void OnDrawGizmosSelected() {
        for (var x = 0; x < mapInfo.MapTurretPlacementCells.GetLength(0); x++) {
            for (var y = 0; y < mapInfo.MapTurretPlacementCells.GetLength(1); y++) {
                Gizmos.color = mapInfo.MapTurretPlacementCells[x, y] ? Color.green : Color.red;
                var point = GetNearestPointOnGrid(new Vector3(
                    transform.position.x + x, 
                    transform.position.y + y, 
                    0f));
                // Gizmos.DrawSphere(point, 0.1f);
                Gizmos.DrawWireCube(point, new Vector3(
                    wireCubeSize,
                    wireCubeSize,
                    0.1f));
            }
        }
    }
}