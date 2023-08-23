using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour {
    [SerializeField] private ProjectileController projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private TowerX data;
    
    private SpriteRenderer _spriteRenderer;
    private List<EnemyController> _enemyControllers;
    private float _attackSpeedTimer;
    private bool _finishedRotate;

    public Sprite Sprite => data.Sprite;

    private void Awake() {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _enemyControllers = new List<EnemyController>();
    }

    private void FixedUpdate() {
        var targetEnemy = GetFirstTargetInRange();
        if (targetEnemy == null) {
            RotateTowards(Vector3.up, data.RotateSpeed);
            return;
        }
        
        // rotate towards target
        var dir = targetEnemy.transform.position - transform.position;
        RotateTowards(dir.normalized, data.RotateSpeed);
        
        var angle = Mathf.Atan2(dir.normalized.y, dir.normalized.x) * Mathf.Rad2Deg;
        var angle2 = Mathf.Atan2(transform.up.y, transform.up.x) * Mathf.Rad2Deg;
        if (Mathf.RoundToInt(angle) == Mathf.RoundToInt(angle2)) {
            _finishedRotate = true;
        }
        
        _attackSpeedTimer -= Time.deltaTime;
        if (_attackSpeedTimer <= 0f && _finishedRotate) {
            Attack(targetEnemy, dir);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            var enemy = other.gameObject.GetComponent<EnemyController>();
            _enemyControllers.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            var enemy = other.gameObject.GetComponent<EnemyController>();
            if (enemy != null && _enemyControllers.Contains(enemy))
                _enemyControllers.Remove(enemy);
        }
    }

    private EnemyController GetFirstTargetInRange() {
        if (_enemyControllers.Count == 0) return null;

        var firstEnemy = _enemyControllers[0];
        for (var i = 1; i < _enemyControllers.Count; i++) {
            if (_enemyControllers[i].TotalDistance > firstEnemy.TotalDistance) {
                firstEnemy = _enemyControllers[i];
            }
        }

        return firstEnemy;
    }

    private void RotateTowards(Vector3 target, float speed) {
        transform.up = Vector3.MoveTowards(transform.up, target, speed * Time.deltaTime);
    }
    
    private void Attack(EnemyController enemy, Vector3 dir) {
        if (_enemyControllers.Count == 0) return;
        
        // shoot at target
        FireProjectile(enemy, dir);

        // reset attack speed
        _attackSpeedTimer = data.AttackSpeed;
    }

    private void FireProjectile(EnemyController enemy, Vector3 dir) {
        var projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        projectile.Setup(enemy, dir, data.ProjectileDamage, data.ProjectileSpeed);
    }

    public Sprite GetSprite() {
        return _spriteRenderer.sprite;
    }
}