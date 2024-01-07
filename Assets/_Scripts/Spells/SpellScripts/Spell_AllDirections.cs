using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class Spell_AllDirections : SpellsProjectiles
{
    public override void OnSpawn(int spellIdx, SO_Spells _spellData)
    {
        base.OnSpawn(spellIdx, _spellData);

        Vector2 direction = Vector2.up;

        switch (spellIdx)
        {
            case 0:
                direction = Vector2.up;
                break;
            case 1:
                direction = Vector2.right;
                break;
            case 2:
                direction = Vector2.down;
                break;
            case 3:
                direction = Vector2.left;
                break;
        }

        MoveStraightInDirection(direction);
    }

    protected override void MoveStraightInDirection(Vector2 direction)
    {
        base.MoveStraightInDirection(direction);
    }
 
    public override void OnTriggerEnter2D(Collider2D _collision)
    {
        base.OnTriggerEnter2D(_collision);

        health -= 1;
        if (health <= 0) Deactivate();
    }
}
