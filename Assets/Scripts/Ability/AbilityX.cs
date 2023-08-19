using System;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Ability")]
public class AbilityX : ScriptableObject {
    [SerializeField] private string abilityName;
    [SerializeField] private float cooldown;
    [SerializeField] private Sprite iconSprite;
    [SerializeField] private TargetingType targetingType;
    [SerializeField] private TargetingMultiplicity targetingMultiplicity;
    [SerializeField] private GameObject selectedAbilityPrefab;

    public TargetingMultiplicity TargetingMultiplicity => targetingMultiplicity;

    public TargetingType TargetingType => targetingType;

    public Sprite IconSprite => iconSprite;

    public float Cooldown => cooldown;

    public string AbilityName => abilityName;
    public GameObject SelectedAbilityPrefab => selectedAbilityPrefab;
}