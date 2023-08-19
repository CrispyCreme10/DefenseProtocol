using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class CooldownManager : SerializedMonoBehaviour {
    public Action<int, float> OnCooldownChange;
    
    [ReadOnly]
    private Dictionary<int, CooldownData> _cooldowns;

    public void Setup(IEnumerable<(int, float)> cooldownObjs) {
        _cooldowns = cooldownObjs.ToDictionary(tuple => tuple.Item1, tuple => new CooldownData(tuple.Item2, false));
    }

    private void Update() {
        foreach (var key in _cooldowns.Keys) {
            if (!_cooldowns[key].isActive) continue;
            
            var cooldownData = _cooldowns[key];
            cooldownData.cooldown -= Time.deltaTime;
            OnCooldownChange?.Invoke(key, cooldownData.cooldown);

            if (cooldownData.cooldown <= 0) {
                cooldownData.isActive = false;
            }
        }
    }

    public void StartCooldown(int key) {
        if (_cooldowns.TryGetValue(key, out var cooldown)) {
            cooldown.isActive = true;
        }
    }

    public bool IsCooldownActive(int key) {
        return _cooldowns.TryGetValue(key, out var cooldown) && cooldown.isActive;
    }
    
    [Serializable]
    private struct CooldownData {
        public float cooldown;
        public bool isActive;

        public CooldownData(float cooldown, bool isActive) {
            this.cooldown = cooldown;
            this.isActive = isActive;
        }
    }
}