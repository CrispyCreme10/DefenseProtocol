using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityManager : MonoBehaviour {
    public Action OnDeselectAbility;
    
    [SerializeField] private Camera sceneCamera;

    private WaveCooldownManager _waveCooldownManager;
    private Transform _selectedAbilityTransform;
    private int _selectedAbilityIndex = -1;

    public WaveCooldownManager WaveCooldownManager => _waveCooldownManager;

    private void Awake() {
        SetupAbilityCooldowns();
    }

    private void OnEnable() {
        Singleton.Instance.BattleManager.OnWaveChange += WaveChange;
    }

    private void OnDisable() {
        Singleton.Instance.BattleManager.OnWaveChange -= WaveChange;
    }

    private void Update() {
        // move selected ability transform
        if (_selectedAbilityTransform == null) return;
        _selectedAbilityTransform.position = GetMousePositionAdj();

        if (!Input.GetMouseButtonDown(0)) return;
        ActivateAbility();
    }

    private void SetupAbilityCooldowns() {
        _waveCooldownManager = new WaveCooldownManager();
        var abilitiesFormatted = Singleton.Instance.PlayerManager.Abilities.Select((a, index) =>
            (index, Mathf.FloorToInt(a.WaveCooldown)));
        _waveCooldownManager.Setup(abilitiesFormatted);
    }

    public void SelectAbility(int abilityIndex) {
        if (!_waveCooldownManager.IsCooldownActive(abilityIndex)) {
            SetSelectedAbility(Singleton.Instance.PlayerManager.Abilities[abilityIndex].SelectedAbilityPrefab,
                abilityIndex);
        }
    }

    public void StartCooldown(int abilityIndex) {
        _waveCooldownManager.StartCooldown(abilityIndex);
    }

    public void SetSelectedAbility(GameObject selectedAbilityPrefab, int index) {
        var selectedAbilityGameObject = Instantiate(selectedAbilityPrefab, GetMousePositionAdj(), Quaternion.identity);
        _selectedAbilityTransform = selectedAbilityGameObject.transform;
        _selectedAbilityIndex = index;
    }

    public void UnsetSelectedAbility() {
        if (_selectedAbilityTransform == null) return;
        Destroy(_selectedAbilityTransform.gameObject);
        _selectedAbilityTransform = null;
        _selectedAbilityIndex = -1;
        OnDeselectAbility?.Invoke();
    }

    private void ActivateAbility() {
        if (Singleton.Instance.PlayerManager.Abilities[_selectedAbilityIndex].name == "LightSnare") {
            var comp = _selectedAbilityTransform.GetComponent<LightSnareAbility>();
            foreach (var enemyController in comp.EnemiesInRange) {
                Debug.Log(enemyController.name);
                enemyController.StartStun(comp.StunDuration);
            }
        }

        StartCooldown(_selectedAbilityIndex);
        UnsetSelectedAbility();
    }
    
    private void WaveChange(int currentWave, int totalWaves) {
        _waveCooldownManager.ReduceAllActiveCooldowns(1);
    }

    private Vector3 GetMousePositionAdj() {
        var pos = sceneCamera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;
        return pos;
    }
}