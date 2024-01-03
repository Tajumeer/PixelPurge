using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class Spells_Projectiles : Spells
{
    protected SO_Spells spellData;
    protected SO_SpellProjectiles spellProjectileData;

    protected float health;

    public virtual void OnSpawn(int _spellIdx, SO_Spells _spellData)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        spellData = _spellData;
        spellProjectileData = spellData.projectileData;

        StartCoroutine(DeleteTimer());

        health = spellProjectileData.enemyHitPoints;
    }

    protected virtual void MoveStraightInDirection(Vector2 direction)
    {
        // Move the Spell with the given speed
        rb.AddRelativeForce(direction * spellData.speed, ForceMode2D.Impulse);
    }

    public virtual void OnCollisionEnter2D(Collision2D _collision)
    {
        // if an enemy got hit by the spell
        if (!_collision.gameObject.CompareTag("Enemy")) return;

        _collision.gameObject.TryGetComponent(out IDamagable character);
        character.GetDamage(spellData.damage);
    }

    /// <summary>
    /// Deletes the Object when the lifetime ends
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator DeleteTimer()
    {
        yield return new WaitForSeconds(spellData.lifetime);
        Deactivate();
    }
}
