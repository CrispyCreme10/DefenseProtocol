using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSnareAbility : MonoBehaviour {
    [SerializeField] private float stunDuration;
    
    private List<EnemyController> _enemiesInRange;

    public List<EnemyController> EnemiesInRange => _enemiesInRange;
    public float StunDuration => stunDuration;
    
    private void Awake() {
        _enemiesInRange = new List<EnemyController>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy")) {
            _enemiesInRange.Add(other.GetComponent<EnemyController>());
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Enemy")) {
            _enemiesInRange.Remove(other.GetComponent<EnemyController>());
        }
    }
}
