using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spells", menuName = "PixelPurge/Spells/Spells", order = 1)]
public class SO_Spells : ScriptableObject
{
    [Header("Insert the matching ScriptbleObjects")]
    public SO_SpellProjectiles projectileData;

    [Header("UI Stuff")]
    public string spellname;
    // icon
    public string description;

    [Space]

    [Header("General Stats")]
    public float damage;
    public float lifetime;
    public float speed;
    public float cd;
    public float radius;
}
