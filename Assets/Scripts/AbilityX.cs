using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Ability")]
public class AbilityX : ScriptableObject {
    [SerializeField] private string abilityName;
    [SerializeField] private float cooldown;
    [SerializeField] private Sprite iconSprite;
    [SerializeField] private TargetingType targetingType;
    [SerializeField] private TargetingMultiplicity targetingMultiplicity;

    public TargetingMultiplicity TargetingMultiplicity => targetingMultiplicity;

    public TargetingType TargetingType => targetingType;

    public Sprite IconSprite => iconSprite;

    public float Cooldown => cooldown;

    public string AbilityName => abilityName;

    public VisualElement OnSelected() {
        // not all abilities have this
        // what happens when applicable abilities need another 1..N actions to fully activate the ability

        return name switch {
            "LightSnare" => OnSelectedLightSnare(),
            _ => new VisualElement()
        };
    }

    public void Activate() {
        // what happens when the ability is activated from the UI
        // can: spawn something that moves and causes damage or just interacts with game systems behind the scenes
        //  to update various kinds of GameObjects' values
    }

    private VisualElement OnSelectedLightSnare() {
        var element = new VisualElement {
            name = "LightSnareSelected"
        };
        
        element.AddToClassList("AbilitySelected");
        
        return element;
    }

    private void ActivateLightSnare() {
        
    }
}