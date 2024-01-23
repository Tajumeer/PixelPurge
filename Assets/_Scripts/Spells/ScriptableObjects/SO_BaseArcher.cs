using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

[CreateAssetMenu(fileName = "BaseArcher", menuName = "PixelPurge/Spells/BaseArcher", order = 1)]
public class SO_BaseArcher : ScriptableObject
{
    [Header("UI Stuff")]
    public string SpellName;
    // icon
    public string SpellDescription;
    public List<AudioClip> SpellSFX;

    [Space]

    [Header("General Stats")]
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
