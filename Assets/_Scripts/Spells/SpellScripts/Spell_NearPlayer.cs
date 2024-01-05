using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class Spell_NearPlayer : SpellsProjectiles
{
    public override void OnSpawn(int spellIdx, SO_Spells _spellData)
    {
        base.OnSpawn(spellIdx, _spellData);

        // Move the Spell with the given speed
        rb.AddRelativeForce(Vector2.down * spellData.speed, ForceMode2D.Impulse);
    }

    public override void OnCollisionEnter2D(Collision2D _collision)
    {
        base.OnCollisionEnter2D(_collision);
    }
}
