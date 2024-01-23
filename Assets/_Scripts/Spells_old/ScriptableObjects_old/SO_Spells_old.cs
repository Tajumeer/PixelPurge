using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

[CreateAssetMenu(fileName = "Spells", menuName = "PixelPurge/Spells_old/Spells", order = 1)]
public class SO_Spells_old : ScriptableObject
{
    [Header("Insert the matching ScriptbleObjects")]
    public SO_SpellProjectiles_old projectileData;

    [Header("UI Stuff")]
    public string spellName;
    // icon
    public string spellDescription;
    public List<AudioClip> spellSFX;

    [Space]

    [Header("General Stats")]
    public float damage;
    public float lifetime;
    public float speed;
    public float cd;
    public float radius;
}