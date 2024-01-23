using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

[CreateAssetMenu(fileName = "SpellUpgrades", menuName = "PixelPurge/Spells_old/SpellUpgrades", order = 0)]
public class SO_SpellUpgrades_old : ScriptableObject
{
    [Header("The Array Objects are the upgrade levels: \nDamage[0] is the damage in Level 1 of a spell and so on \nUpgrades in 0.XX%")]

    public float[] Damage;
    public int[] ProjectileCount;
}
