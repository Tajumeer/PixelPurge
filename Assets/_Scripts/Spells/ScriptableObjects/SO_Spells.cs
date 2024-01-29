using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

[CreateAssetMenu(fileName = "Spells", menuName = "PixelPurge/Spells/Spells", order = 1)]
public class SO_Spells : ScriptableObject
{
    [Header("UI Stuff")]
    public string SpellName;
    public Sprite SpellIcon;
    public string SpellDescription;
    public List<AudioClip> SpellSFX;

    [Space]

    [Header("General Stats")]
    public int Level = 0;
    public int MaxLevel;
    [Space]
    public float[] Damage;
    public float[] Lifetime;
    public float[] Speed;
    public float[] Cd;
    public float[] Radius;

    [Header("Projectile Stats")]
    public int[] Amount;
    /// <summary>
    /// How many Enemys can the spell hit until it dies
    /// </summary>
    public float[] EnemyHitPoints;
}
