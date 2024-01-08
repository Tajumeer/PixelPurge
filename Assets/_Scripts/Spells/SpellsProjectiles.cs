using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class SpellsProjectiles : Spells
{
    protected SO_SpellProjectiles spellProjectileData;

    protected float health;

    /// <summary>
    /// Get Rigidbody, set SpellData
    /// and start DeleteTimer
    /// </summary>
    /// <param name="_spellIdx"></param>
    /// <param name="_spellData"></param>
    public override void OnSpawn(int _spellIdx, SO_Spells _spellData)
    {
        base.OnSpawn(_spellIdx, _spellData);

        spellProjectileData = spellData.projectileData;

        StartCoroutine(DeleteTimer());

        health = spellProjectileData.enemyHitPoints;
    }

    /// <summary>
    /// Moves the GameObject in the direction with the data-speed
    /// </summary>
    /// <param name="direction"></param>
    protected virtual void MoveStraightInDirection(Vector2 direction)
    {
        // Move the Spell with the given speed
        rb.AddRelativeForce(direction * spellData.speed, ForceMode2D.Impulse);
    }
    public virtual void OnTriggerEnter2D(Collider2D _collision)
    {
        // if an enemy got hit by the spell
        if (!_collision.gameObject.CompareTag("Enemy")) return;

        _collision.gameObject.GetComponent<IDamagable>().GetDamage(spellData.damage);
    }
  

    /// <summary>
    /// Deletes the Object when the lifetime ends
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator DeleteTimer()
    {
        yield return new WaitForSeconds(spellData.lifetime);

        DeactivateSpell();
    }

    protected virtual void DeactivateSpell()
    {
        StopAllCoroutines();

        if (gameObject.GetComponent<Spell_NearPlayer>() != null) gameObject.GetComponent<Spell_NearPlayer>().enabled = false;
        if (gameObject.GetComponent<Spell_AllDirections>() != null) gameObject.GetComponent<Spell_AllDirections>().enabled = false;

        Deactivate();
    }
}
