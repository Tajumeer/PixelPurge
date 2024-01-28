using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

[CreateAssetMenu(fileName = "AllDirections", menuName = "PixelPurge/Spells/AllDirections", order = 8)]
public class SO_AllDirections : ScriptableObject
{
    [Header("UI Stuff")]
    public string SpellName;
    public Sprite SpellIcon;
    public string SpellDescription;
    public List<AudioClip> SpellSFX;

    [Space]

    [Header("General Stats")]
    public int Level = 0;
    [Space]
    public float Damage;
    public float Lifetime;
    public float Speed;
    public float Cd;
    public float Radius;

    [Header("Projectile Stats")]
    /// <summary>
    /// How many Enemys can the spell hit until it dies
    /// </summary>
    public float EnemyHitPoints;
    public int Amount;
}
