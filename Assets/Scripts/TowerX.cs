using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tower")]
public class TowerX : ScriptableObject {
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private int projectileDamage = 10;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private Sprite sprite;

    public float ProjectileSpeed => projectileSpeed;

    public int ProjectileDamage => projectileDamage;

    public float RotateSpeed => rotateSpeed;

    public float AttackSpeed => attackSpeed;
    public Sprite Sprite => sprite;
}
