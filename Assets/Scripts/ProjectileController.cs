using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {
    private Rigidbody2D _rb;

    private EnemyController _enemy;
    private int _damage;
    private float _speed;

    public int Damage => _damage;
    
    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        if (_enemy == null) {
            Destroy(gameObject);
            return;
        }
         
        var dir = _enemy.transform.position - transform.position;
        _rb.velocity =  _speed * dir.normalized;
        transform.up = Vector3.MoveTowards(transform.up, dir.normalized, 100f);
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            Destroy(gameObject);
        }
    }

    public void Setup(EnemyController enemy, Vector3 dir, int damage, float speed) {
        _enemy = enemy;
        transform.up = Vector3.MoveTowards(transform.up, dir.normalized, 100f);
        _damage = damage;
        _speed = speed;
    }
}
