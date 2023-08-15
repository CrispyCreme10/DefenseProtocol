using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {
    [SerializeField] private List<StackTower> towers;

    public List<StackTower> Towers => towers;
}

[Serializable]
public class StackTower {
    [SerializeField] private TowerController _towerController;
    [SerializeField] private Sprite _towerSprite;
    [SerializeField] private int amount;
    
    public int Amount => amount;
    public TowerController TowerController => _towerController;
    public Sprite TowerSprite => _towerSprite;
}