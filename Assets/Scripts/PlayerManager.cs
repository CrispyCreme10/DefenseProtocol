using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour {
    public static Action OnTowersChange;

    [SerializeField] private List<AbilityX> abilities;
    [SerializeField] private List<StackTower> towers;
    [SerializeField] private List<UtilityX> utilities;
    [SerializeField] private int upgradePoints;

    private List<TowerController> _placedTowers;

    public List<AbilityX> Abilities => abilities;
    public List<StackTower> Towers => towers;
    public List<UtilityX> Utilities => utilities;
    public int UpgradePoints => upgradePoints;

    private void Awake() {
        _placedTowers = new List<TowerController>();
    }

    public void SpawnTower(int index, Vector2 position) {
        var tower = Instantiate(towers[index].TowerController, position, Quaternion.identity);
        _placedTowers.Add(tower);
        towers[index].DecrementAmount();
        if (towers[index].Amount == 0) {
            towers.RemoveAt(index);
        }
        OnTowersChange?.Invoke();
    }
}

[Serializable]
public class StackTower {
    [SerializeField] private TowerController _towerController;
    [SerializeField] private Sprite _towerSprite;
    [SerializeField] private int amount;
    
    public int Amount => amount;
    public TowerController TowerController => _towerController;
    public Sprite TowerSprite => _towerSprite;

    public void DecrementAmount() {
        amount--;
    }
}

public enum TargetingType {
    Freeform,
    Enemy,
    Tower,
    Path
}

public enum TargetingMultiplicity {
    Single,
    Multiple
}