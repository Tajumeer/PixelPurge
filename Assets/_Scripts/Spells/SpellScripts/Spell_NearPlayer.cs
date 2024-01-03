using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class Spell_NearPlayer : Spells_Projectiles
{
    public override void OnSpawn(int spellIdx, SO_Spells _spellData)
    {
        base.OnSpawn(spellIdx, _spellData);

        // Update position and rotation
        rb.velocity = new Vector2(0f, 0f);
        rb.position = new Vector2(transform.position.x, transform.position.y);
        rb.rotation = transform.rotation.z;

        // Move the Spell with the given speed
        rb.AddRelativeForce(Vector2.down * spellData.speed, ForceMode2D.Impulse);
    }

    public override void OnCollisionEnter2D(Collision2D _collision)
    {
        base.OnCollisionEnter2D(_collision);
    }
}
