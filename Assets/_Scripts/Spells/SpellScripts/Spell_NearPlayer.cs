using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class Spell_NearPlayer : PoolObject<Spell_NearPlayer>
{
    private Rigidbody2D rb;
    private SO_NearPlayer spellData;

    /// <summary>
    /// Get & reset Rigidbody, 
    /// start Lifetime & DeleteTimer,
    /// move
    /// </summary>
    /// <param name="_spellIdx"></param>
    public void OnSpawn(SO_NearPlayer _spellData)
    {
        InitRigidbody();

        spellData = _spellData;

        // Start Lifetime
        StartCoroutine(DeleteTimer());

        Move();
    }

    /// <summary>
    /// Move away from the player in the given direction for this projecitle
    /// </summary>
    /// <param name="_spellIdx"></param>
    private void Move()
    {
        rb.AddRelativeForce(Vector2.down * spellData.Speed, ForceMode2D.Impulse);
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
