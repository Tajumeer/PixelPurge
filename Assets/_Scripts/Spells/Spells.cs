using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class Spells : PoolObject<Spells>
{
    [Header("UI Stuff")]
    public string spellname;
    public string description;
    // icon

    [Space]

    protected Rigidbody2D rb;
    protected float damage;
    protected float lifetime;
    protected float speed;

    protected virtual void InitStats(SO_Spells spellData)
    {
        spellname = spellData.name;
        description = spellData.description;
        damage = spellData.damage;
        lifetime = spellData.lifetime;
        speed = spellData.speed;
    }
}
