using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyController : MonoBehaviour {
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private int maxHealth = 10;

    public float TotalDistance => moveSpeed * _timeAlive;
    
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private int _currentPathPointIndex = 1;
    private bool _canMove = true;
    private float _timeAlive;
    private IEnumerator _stunCoroutine;
    
    [SerializeField]
    [ReadOnly]
    private int _health;
    
    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _health = maxHealth;
    }

    private void Update() {
        _timeAlive += Time.deltaTime;
    }

    private void FixedUpdate() {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Projectile")) {
            var damage = other.GetComponent<ProjectileController>().Damage;
            TakeDamage(damage);
        }
    }

    private void Move() {
        if (!_canMove) return;
        
        var nextPoint = Singleton.Instance.MapManager.PathPoints[_currentPathPointIndex];
        var distance = Vector3.Distance(transform.position, nextPoint);
        if (distance <= 0.01) {
            _currentPathPointIndex++;
            if (_currentPathPointIndex >= Singleton.Instance.MapManager.PathPoints.Length) {
                // reached end of path
                Singleton.Instance.BattleManager.DecrementLives();
                DestroySelf();
            }
        } else {
            var dir = (new Vector3(nextPoint.x, nextPoint.y, 0f) - transform.position).normalized;
            _rb.velocity = new Vector3(dir.x * moveSpeed, dir.y * moveSpeed, 0f);
        }
    }

    private void TakeDamage(int damage) {
        _health -= damage;
        if (_health <= 0) {
            DestroySelf();
        }
    }

    public void StartStun(float duration) {
        _stunCoroutine = Stun(duration);
        StartCoroutine(_stunCoroutine);
    }

    private IEnumerator Stun(float duration) {
        // apply stun effect
        _canMove = false;
        var defaultColor = _spriteRenderer.color;
        var previousVelocity = _rb.velocity;
        _spriteRenderer.color = Color.yellow;
        _rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(duration);
        _canMove = true;
        _rb.velocity = previousVelocity;
        _spriteRenderer.color = defaultColor;
    }

    private void DestroySelf() {
        Singleton.Instance.BattleManager.MoveDestroyedEnemy(this);
        if (_stunCoroutine != null) {
            StopCoroutine(_stunCoroutine);
        }
        Destroy(gameObject);
    }
}
