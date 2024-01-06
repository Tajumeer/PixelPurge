using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class Spells : PoolObject<Spells>
{
    protected Rigidbody2D rb;
    protected SO_Spells spellData;

    /// <summary>
    /// Get and reset Rigidbody and set SpellData
    /// </summary>
    /// <param name="_spellIdx"></param>
    /// <param name="_spellData"></param>
    public virtual void OnSpawn(int _spellIdx, SO_Spells _spellData)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        // Reset rigidbody
        rb.velocity = new Vector2(0f, 0f);
        rb.position = new Vector2(transform.localPosition.x, transform.localPosition.y);
        rb.rotation = transform.localRotation.z;

        spellData = _spellData;
    }
}
