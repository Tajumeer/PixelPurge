using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

[CreateAssetMenu(fileName = "NearPlayer", menuName = "PixelPurge/Spells/NearPlayer", order = 9)]
public class SO_NearPlayer : ScriptableObject
{
    [Header("UI Stuff")]
    public string SpellName;
    public Sprite SpellIcon;
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
    public int Amount;
}
