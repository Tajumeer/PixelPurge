using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

[CreateAssetMenu(fileName = "SpellData", menuName = "PixelPurge/SpellData", order = 2)]
public class SO_FourDirection : ScriptableObject
{
    public float Damage;

    public float Speed;

    public float Lifetime;

    /// <summary>
    /// How many Enemys can the spell hit until it dies
    /// </summary>
    public float EnemyHitPoints;
}
