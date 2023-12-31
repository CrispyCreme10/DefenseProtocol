using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance { get; private set; }
    public PlayerManager PlayerManager { get; private set; }
    public MapManager MapManager { get; private set; }
    public BattleManager BattleManager { get; private set; }
    public AbilityManager AbilityManager { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }

        Instance = this;
        PlayerManager = GetComponentInChildren<PlayerManager>();
        MapManager = GetComponentInChildren<MapManager>();
        BattleManager = GetComponentInChildren<BattleManager>();
        AbilityManager = GetComponentInChildren<AbilityManager>();
    }
}
