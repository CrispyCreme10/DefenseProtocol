using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

public class WaveCooldownManager {
    [ReadOnly] public Action<int, float> OnCooldownChange;

    private Dictionary<int, CooldownData> _cooldowns;
    
    public void Setup(IEnumerable<(int, int)> cooldownObjs) {
        _cooldowns = cooldownObjs.ToDictionary(tuple => tuple.Item1, tuple => new CooldownData(tuple.Item2, false));
    }
    
    public void StartCooldown(int key) {
        if (_cooldowns.TryGetValue(key, out var cooldown)) {
            cooldown.isActive = true;
            
            OnCooldownChange?.Invoke(key, cooldown.cooldown);
        }
    }

    public void ReduceAllActiveCooldowns(int reductionValue) {
        foreach (var key in _cooldowns.Keys) {
            if (!_cooldowns[key].isActive) continue;
            
            ReduceCooldown(key, reductionValue);
        }
    }

    public void ReduceCooldown(int key, int reductionValue) {
        if (!_cooldowns.TryGetValue(key, out var cooldown)) return;
        
        cooldown.cooldown -= reductionValue;
        OnCooldownChange?.Invoke(key, cooldown.cooldown);
            
        if (cooldown.cooldown <= 0) {
            cooldown.isActive = false;
            cooldown.cooldown = cooldown.cooldownReset;
        }
    }

    public bool IsCooldownActive(int key) {
        return _cooldowns.TryGetValue(key, out var cooldown) && cooldown.isActive;
    }
    
    [Serializable]
    private class CooldownData {
        public int cooldownReset;
        public int cooldown;
        public bool isActive;

        public CooldownData(int cooldown, bool isActive) {
            cooldownReset = cooldown;
            this.cooldown = cooldown;
            this.isActive = isActive;
        }
    }
}