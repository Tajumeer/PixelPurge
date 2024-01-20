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


        if (_spellData.spellSFX != null)
        {
            AudioManager.Instance.PlaySound(_spellData.spellSFX[Random.Range(0, _spellData.spellSFX.Count)]);
        }


        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 arrowDirection = (mousePosition - transform.position).normalized;

        MoveStraightInDirection(new Vector2(arrowDirection.x, arrowDirection.y).normalized);

        transform.Rotate(0f, 0f, Mathf.Atan2(arrowDirection.y, arrowDirection.x) * Mathf.Rad2Deg);

        /**** Sven End ****/
    }


    public override void OnTriggerEnter2D(Collider2D _collision)
    {
        base.OnTriggerEnter2D(_collision);
    }
}
