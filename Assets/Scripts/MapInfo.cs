using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour {
    [SerializeField] private Vector2[] turretPoints;
    [SerializeField] private Vector2[] pathPoints;

    public Vector2[] TurretPoints => turretPoints;
    public Vector2[] PathPoints => pathPoints;
    
    private void OnDrawGizmos() {
        foreach (var turretPoint in turretPoints) {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(turretPoint, new Vector3(1f, 1f, 0f));
        }
        
        foreach (var pathPoint in pathPoints) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(pathPoint, new Vector3(1f, 1f, 0f));
        }
    }
}
