using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

[CreateAssetMenu(fileName = "ActiveSpells", menuName = "PixelPurge/Spells/ActiveSpells", order = 2)]
public class SO_ActiveSpells : ScriptableObject
{
    /** if something is added/removed from here, it also has to be removed from:
     *  the Scriptable Object "SO_ActiveSpellNames" 
     *  and the Script "LevelUpManager" in the Function for the Active Spell Cards
     */

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
    public int[] ProjectileAmount;
    /// <summary>
    /// How many Enemys can the spell pass trough it dies
    /// </summary>
    public float[] Pierce;
    public float[] Bounce;
}
