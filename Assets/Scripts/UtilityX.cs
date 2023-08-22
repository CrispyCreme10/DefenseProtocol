using UnityEngine;

[CreateAssetMenu(menuName = "Utility")]
public class UtilityX : ScriptableObject {
    [SerializeField] private string abilityName;
    [SerializeField] private int maxStackSize;
    [SerializeField] private Sprite iconSprite;
    [SerializeField] private TargetingType targetingType;
    [SerializeField] private TargetingMultiplicity targetingMultiplicity;
    [SerializeField] private GameObject selectedUtilityPrefab;
    
    public GameObject SelectedUtilityPrefab => selectedUtilityPrefab;

    public TargetingMultiplicity TargetingMultiplicity => targetingMultiplicity;

    public TargetingType TargetingType => targetingType;

    public Sprite IconSprite => iconSprite;

    public int MaxStackSize => maxStackSize;

    public string AbilityName => abilityName;
}