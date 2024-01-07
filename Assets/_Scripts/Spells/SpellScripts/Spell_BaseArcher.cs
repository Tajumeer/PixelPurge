using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya, Sven

public class Spell_BaseArcher : SpellsProjectiles
{
    public override void OnSpawn(int spellIdx, SO_Spells _spellData)
    {
        base.OnSpawn(spellIdx, _spellData);

        /**** Sven Start ****/

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 arrowDirection = (mousePosition - transform.position).normalized;

        MoveStraightInDirection(arrowDirection);
        
        transform.Rotate(0f, 0f, Mathf.Atan2(arrowDirection.y, arrowDirection.x) * Mathf.Rad2Deg);

        /**** Sven End ****/
    }

    public override void OnCollisionEnter2D(Collision2D _collision)
    {
        base.OnCollisionEnter2D(_collision);
    }
}