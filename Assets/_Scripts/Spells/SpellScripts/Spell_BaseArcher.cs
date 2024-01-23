using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya, Sven

public class Spell_BaseArcher : PoolObject<Spell_BaseArcher>
{
    private Rigidbody2D rb;
    private SO_BaseArcher spellData;

    private float health;

    /// <summary>
    /// Get & reset Rigidbody, 
    /// start Lifetime & DeleteTimer,
    /// move
    /// </summary>
    /// <param name="_spellIdx"></param>
    public void OnSpawn(SO_BaseArcher _spellData)
    {
        InitRigidbody();

        spellData = _spellData;

        // Start Lifetime
        StartCoroutine(DeleteTimer());
        health = spellData.EnemyHitPoints;

        /**** Sven Start ****/

        if (spellData.SpellSFX != null)
        {
            //AudioManager.Instance.PlaySound(spellData.SpellSFX[Random.Range(0, spellData.SpellSFX.Count)]);
        }

        Move();

        /**** Sven End ****/
    }

    /// <summary>
    /// Move away from the player in the given direction for this projecitle -BySven
    /// </summary>
    /// <param name="_spellIdx"></param>
    private void Move()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 arrowDirection = (mousePosition - transform.position).normalized;

        Vector2 direction = new Vector2(arrowDirection.x, arrowDirection.y).normalized;
        rb.AddRelativeForce(direction * spellData.Speed, ForceMode2D.Impulse);

        transform.Rotate(0f, 0f, Mathf.Atan2(arrowDirection.y, arrowDirection.x) * Mathf.Rad2Deg);
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
