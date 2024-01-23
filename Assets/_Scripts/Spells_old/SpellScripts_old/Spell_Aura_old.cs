using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Aura_old : Spells_old
{
    public override void OnSpawn(int _spellIdx, SO_Spells_old _spellData)
    {
        base.OnSpawn(_spellIdx, _spellData);
    }

    protected void OnTriggerStay2D(Collider2D _collision)
    {
        // if an enemy got hit by the spell
        if (!_collision.gameObject.CompareTag("Enemy")) return;

        _collision.gameObject.TryGetComponent(out IDamagable character);
        character.GetDamage(spellData.damage);
    }
}
