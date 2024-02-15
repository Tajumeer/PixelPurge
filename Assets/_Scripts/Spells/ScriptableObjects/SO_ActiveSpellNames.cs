using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

[CreateAssetMenu(fileName = "SpellNames", menuName = "PixelPurge/Spells/SpellNames", order = 5)]
public class SO_ActiveSpellNames : ScriptableObject
{
    [Header("These are the used descriptions when a spell can be upgraded \n(e.g. + 5% Damage)")]

    [Header("General Stats")]
    public string Damage;
    public string Lifetime;
    public string Speed;
    public string Cd;
    public string Radius;

    [Header("Projectile Stats")]
    public string ProjectileAmount;
    public string Bounce;
    public string Pierce;
}
