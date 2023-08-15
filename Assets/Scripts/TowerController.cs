using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour {
    private List<EnemyController> _enemyControllers;

    private void Awake() {
        _enemyControllers = new List<EnemyController>();
    }

    private void Update() {
        Attack();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            _enemyControllers.Add(other.gameObject.GetComponent<EnemyController>());
        }
    }

    private Vector3? GetFirstTargetInRange() {
        if (_enemyControllers.Count == 0) return null;

        var firstEnemy = _enemyControllers[0];
        for (var i = 1; i < _enemyControllers.Count; i++) {
            if (_enemyControllers[i].TotalDistance > firstEnemy.TotalDistance) {
                firstEnemy = _enemyControllers[i];
            }
        }

        return firstEnemy.transform.position;
    }

    private void Attack() {
        if (_enemyControllers.Count == 0) return;

        var targetPos = GetFirstTargetInRange();
        
        // rotate towards target
        
        // shoot at target
        
    }
}
