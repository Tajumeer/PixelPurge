using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

[CreateAssetMenu(fileName = "SpellProjectiles", menuName = "PixelPurge/Spells/SpellProjectiles", order = 1)]
public class SO_SpellProjectiles : ScriptableObject
{
    [Header("Projectile Stats")]
    public int amount;
    /// <summary>
    /// How many Enemys can the spell hit until it dies
    /// </summary>
    public float enemyHitPoints;
}
