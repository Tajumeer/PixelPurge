using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Aura : Spells
{
    public override void OnSpawn(int _spellIdx, SO_Spells _spellData)
    {
        base.OnSpawn(_spellIdx, _spellData);
    }

    public void OnCollisionStay2D(Collision2D _collision)
    {
        // if an enemy got hit by the spell
        if (!_collision.gameObject.CompareTag("Enemy")) return;

        _collision.gameObject.TryGetComponent(out IDamagable character);
        character.GetDamage(spellData.damage);
    }
}
