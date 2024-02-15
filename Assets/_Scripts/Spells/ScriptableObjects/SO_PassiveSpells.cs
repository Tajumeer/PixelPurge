using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

[CreateAssetMenu(fileName = "PassiveSpells", menuName = "PixelPurge/Spells/PassiveSpells", order = 3)]
public class SO_PassiveSpells : ScriptableObject
{
    [Header("UI Stuff")]
    public string SpellName;
    public Sprite SpellIcon;
    [Tooltip("Is shown when the Spell can be upgraded e.g. +5% Value")]
    public string SpellUpgradeDescription;

    [Space]

    [Header("General Stats")]
    public int Level = 0;
    public int MaxLevel;

    public float[] Stat;
}
