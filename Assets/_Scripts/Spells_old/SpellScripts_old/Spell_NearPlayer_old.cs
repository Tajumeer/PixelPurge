using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class Spell_NearPlayer_old : SpellsProjectiles_old
{
    public override void OnSpawn(int spellIdx, SO_Spells_old _spellData)
    {
        base.OnSpawn(spellIdx, _spellData);

        // Move the Spell with the given speed
        rb.AddRelativeForce(Vector2.down * spellData.speed, ForceMode2D.Impulse);
    }
 
    public override void OnTriggerEnter2D(Collider2D _collision)
    {
        base.OnTriggerEnter2D(_collision);
    }
}
