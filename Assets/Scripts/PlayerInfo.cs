using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {
    public static Action OnTowersChange;
    
    [SerializeField] private List<StackTower> towers;
    [SerializeField] private int upgradePoints;

    private List<TowerController> _placedTowers;
    
    public List<StackTower> Towers => towers;

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

[Serializable]
public class Ability {
    [SerializeField] private string abilityName;
    [SerializeField] private float cooldown;
}