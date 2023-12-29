using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class Spells_Projectiles : Spells
{
    protected int amount;
    protected float enemyHitPoints;

    protected float health;

    public virtual void OnSpawn(int spellIdx, SO_Spells spellData)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        InitStats(spellData);

        StartCoroutine(DeleteTimer());

        health = enemyHitPoints;
    }

    protected virtual void MoveStraightInDirection(Vector2 direction)
    {
        // Move the Spell with the given speed
        rb.AddRelativeForce(direction * speed, ForceMode2D.Impulse);
    }

    public virtual void OnCollisionEnter2D(Collision2D _collision)
    {
        // if an enemy got hit by the spell
        if (!_collision.gameObject.CompareTag("Enemy")) return;

        _collision.gameObject.TryGetComponent(out IDamagable character);
        character.GetDamage(damage);
    }

    /// <summary>
    /// Deletes the Object when the lifetime ends
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator DeleteTimer()
    {
        yield return new WaitForSeconds(lifetime);
        Deactivate();
    }

    protected override void InitStats(SO_Spells spellData)
    {
        base.InitStats(spellData);

        amount = spellData.projectileData.amount;
        enemyHitPoints = spellData.projectileData.enemyHitPoints;
    }
}
