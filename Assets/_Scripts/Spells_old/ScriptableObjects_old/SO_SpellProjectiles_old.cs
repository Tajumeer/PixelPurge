using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

[CreateAssetMenu(fileName = "SpellProjectiles", menuName = "PixelPurge/Spells_old/SpellProjectiles", order = 2)]
public class SO_SpellProjectiles_old : ScriptableObject
{
    [Header("Projectile Stats")]
    public int amount;
    /// <summary>
    /// How many Enemys can the spell hit until it dies
    /// </summary>
    public float enemyHitPoints;
}
