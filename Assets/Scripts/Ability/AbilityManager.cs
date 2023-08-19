using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityManager : MonoBehaviour {
    public static Action<int, float> OnAbilityCooldownChange;

    [SerializeField] private Camera sceneCamera;

    private CooldownManager _cooldownManager;
    private Transform _selectedAbilityTransform;
    private int _selectedAbilityIndex = -1;

    private void Start() {
        SetupAbilityCooldowns();
        
        OnAbilityCooldownChange += _cooldownManager.OnCooldownChange;
    }

    private void OnDisable() {
        OnAbilityCooldownChange -= _cooldownManager.OnCooldownChange;
    }

    private void Update() {
        // move selected ability transform
        if (_selectedAbilityTransform == null) return;
        _selectedAbilityTransform.position = GetMousePositionAdj();

        if (!Input.GetMouseButtonDown(0)) return;
        ActivateAbility();
    }

    private void SetupAbilityCooldowns() {
        _cooldownManager = gameObject.AddComponent<CooldownManager>();
        _cooldownManager.Setup(Singleton.Instance.PlayerManager.Abilities.Select((a, index) => (index, a.Cooldown)));
    }

    public void SelectAbility(int abilityIndex) {
        if (!_cooldownManager.IsCooldownActive(abilityIndex)) {
            SetSelectedAbility(Singleton.Instance.PlayerManager.Abilities[abilityIndex].SelectedAbilityPrefab,
                abilityIndex);
        }
    }

    public void StartCooldown(int abilityIndex) {
        _cooldownManager.StartCooldown(abilityIndex);
    }

    public void SetSelectedAbility(GameObject selectedAbilityPrefab, int index) {
        var selectedAbilityGameObject = Instantiate(selectedAbilityPrefab, GetMousePositionAdj(), Quaternion.identity);
        _selectedAbilityTransform = selectedAbilityGameObject.transform;
        _selectedAbilityIndex = index;
    }

    public void UnsetSelectedAbility() {
        Destroy(_selectedAbilityTransform.gameObject);
        _selectedAbilityTransform = null;
        _selectedAbilityIndex = -1;
    }

    private void ActivateAbility() {
        if (name == "LightSnare") {
            var comp = _selectedAbilityTransform.GetComponent<LightSnareAbility>();
            foreach (var enemyController in comp.EnemiesInRange) {
                StartCoroutine(enemyController.Stun(comp.StunDuration));
            }
        }

        StartCooldown(_selectedAbilityIndex);
        UnsetSelectedAbility();
    }

    private Vector3 GetMousePositionAdj() {
        var pos = sceneCamera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;
        return pos;
    }
}