using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

public class BattleManager : MonoBehaviour {
    public static Action OnEndOfWave;
    public static Action<int, int> OnWaveChange;
    public static Action<int> OnLivesChange;
    
    [SerializeField]
    private MapInfo mapInfo;
    [SerializeField] 
    private int totalPlayerLives;
    [SerializeField] 
    [ReadOnly]
    private int playerLives;
    [SerializeField]
    [ReadOnly]
    private int currentWave;
    [SerializeField]
    private List<WaveInfo> waves;

    private bool _battleComplete;
    private float _spawnCountdown;
    private bool _playWave;
    private int _currentWaveEnemyIndex;
    private List<EnemyController> _aliveEnemies;
    private List<EnemyController> _destroyedEnemies;
    
    private void Awake() {
        // init waves
        currentWave = 0;
        playerLives = totalPlayerLives;
        _aliveEnemies = new List<EnemyController>();
        _destroyedEnemies = new List<EnemyController>();
    }

    private void Start() {
        OnWaveChange?.Invoke(currentWave + 1, waves.Count);
        OnLivesChange?.Invoke(playerLives);
    }

    private void Update() {
        if (_battleComplete) return;

        if (_playWave) {
            if (_spawnCountdown <= 0) {
                // spawn enemies in current wave
                var enemyWaveInfo = waves[currentWave].Enemies[_currentWaveEnemyIndex];
                var enemy = Instantiate(enemyWaveInfo.EnemyController,
                    mapInfo.PathPoints[0], Quaternion.identity);
                enemy.Setup(mapInfo, this);
                // for tracking purposes
                _aliveEnemies.Add(enemy);
                // update spawn count
                _spawnCountdown = enemyWaveInfo.DelayToNextEnemy;
                
                // check for end of wave
                _currentWaveEnemyIndex++;
                if (_currentWaveEnemyIndex == waves[currentWave].Enemies.Count) {
                    // done with current wave
                    SetupNextWave();
                }
            }

            _spawnCountdown -= Time.deltaTime;
        }
    }

    public void StartWave() {
        _playWave = true;
    }

    private void IncrementWave() {
        currentWave++;
        OnWaveChange?.Invoke(currentWave + 1, waves.Count);
    }

    private void SetupNextWave() {
        _playWave = false;
        _currentWaveEnemyIndex = 0;
        _destroyedEnemies.Clear();
        IncrementWave();
    }
    
    public void MoveDestroyedEnemy(EnemyController enemyController) {
        if (_aliveEnemies.Contains(enemyController)) {
            _aliveEnemies.Remove(enemyController);
            _destroyedEnemies.Add(enemyController);

            if (_aliveEnemies.Count == 0) {
                // end of wave
                OnEndOfWave?.Invoke();
                // end of battle
                if (currentWave == waves.Count) {
                    EndOfBattle(true);
                }
            }
        }
    }

    public void DecrementLives() {
        playerLives--;
        OnLivesChange?.Invoke(playerLives);
        if (playerLives == 0) {
            // game over
            EndOfBattle(false);
        }
    }

    private void EndOfBattle(bool playerWon) {
        // can allow for time to animate/destroy leftover things
        
        Debug.Log($"GAME OVER: {(playerWon ? "YOU WIN!" : "YOU LOSE")}");
        _battleComplete = true;
        Time.timeScale = 0f;
    }
}

[Serializable]
public class WaveInfo {
    [SerializeField]
    private List<EnemyWaveInfo> enemies;

    public List<EnemyWaveInfo> Enemies => enemies;
}

[Serializable]
public class EnemyWaveInfo {
    [SerializeField]
    private EnemyController enemyController;
    [SerializeField]
    private float delayToNextEnemy = 1f; // seconds

    public float DelayToNextEnemy => delayToNextEnemy;
    public EnemyController EnemyController => enemyController;
}
