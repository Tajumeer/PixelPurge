using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class Spell_AllDirections : PoolObject<Spell_AllDirections>
{
    private Rigidbody2D rb;
    private SO_AllDirections spellData;

    private float health;

    /// <summary>
    /// Get & reset Rigidbody, 
    /// start Lifetime & DeleteTimer,
    /// move
    /// </summary>
    /// <param name="_spellIdx"></param>
    public void OnSpawn(SO_AllDirections _spellData, int _spellIdx)
    {
        InitRigidbody();

        spellData = _spellData;

        // Start Lifetime
        StartCoroutine(DeleteTimer());
        health = spellData.EnemyHitPoints;

        Move(_spellIdx);
    }

    /// <summary>
    /// Move away from the player in the given direction for this projecitle
    /// </summary>
    /// <param name="_spellIdx"></param>
    private void Move(int _spellIdx)
    {
        Vector2 direction = Vector2.up;

        switch (_spellIdx)
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

        rb.AddRelativeForce(direction * spellData.Speed, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Get and reset rigidbody
    /// </summary>
    private void InitRigidbody()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0f, 0f);
        rb.position = new Vector2(transform.position.x, transform.position.y);
        rb.rotation = transform.localRotation.z;
    }

    public void OnTriggerEnter2D(Collider2D _collision)
    {
        // only an enemy can get hit by the spell
        if (!_collision.gameObject.CompareTag("Enemy")) return;

        // the enemy get damage on hit
        _collision.gameObject.GetComponent<IDamagable>().GetDamage(spellData.Damage);

        // and the spell loses duration or dies
        health -= 1;
        if (health <= 0) DeactivateSpell();
    }

    /// <summary>
    /// Deletes the Object when the lifetime ends
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeleteTimer()
    {
        yield return new WaitForSeconds(spellData.Lifetime);

        DeactivateSpell();
    }

    private void DeactivateSpell()
    {
        StopAllCoroutines();

        Deactivate();
    }
}
