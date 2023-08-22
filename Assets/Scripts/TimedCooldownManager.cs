using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class TimedCooldownManager : SerializedMonoBehaviour {
    [ReadOnly] public Action<int, float> OnCooldownChange;
    
    private Dictionary<int, CooldownData> _cooldowns;

    public void Setup(IEnumerable<(int, float)> cooldownObjs) {
        _cooldowns = cooldownObjs.ToDictionary(tuple => tuple.Item1, tuple => new CooldownData(tuple.Item2, false));
    }

    private void Update() {
        foreach (var key in _cooldowns.Keys) {
            if (!_cooldowns[key].isActive) continue;
            
            ReduceCooldown(key);
        }
    }

    public void StartCooldown(int key) {
        if (_cooldowns.TryGetValue(key, out var cooldown)) {
            cooldown.isActive = true;
        }
    }

    private void ReduceCooldown(int key) {
        var cooldownData = _cooldowns[key];
        cooldownData.cooldown -= Time.deltaTime;
        OnCooldownChange?.Invoke(key, cooldownData.cooldown);
            
        // Debug.Log($"{key}: {cooldownData.cooldown}");

        if (cooldownData.cooldown <= 0) {
            cooldownData.isActive = false;
        }
    }

    public bool IsCooldownActive(int key) {
        return _cooldowns.TryGetValue(key, out var cooldown) && cooldown.isActive;
    }
    
    [Serializable]
    private class CooldownData {
        public float cooldown;
        public bool isActive;

        public CooldownData(float cooldown, bool isActive) {
            this.cooldown = cooldown;
            this.isActive = isActive;
        }
    }
}

