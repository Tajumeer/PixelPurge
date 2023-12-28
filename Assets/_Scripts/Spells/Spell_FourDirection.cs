using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

[RequireComponent(typeof(Rigidbody2D))]
public class Spell_FourDirection : PoolObject<Spell_FourDirection>
{
    private Rigidbody2D rb;

    private SO_FourDirection spellData;

    private float health;

    public void Spawn(SO_FourDirection _data, Vector2 direction)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        spellData = _data;
        health = spellData.EnemyHitPoints;

        StartCoroutine(DeleteTimer());


        // Update position and rotation
        rb.velocity = new Vector2(0f, 0f);
        rb.position = new Vector2(transform.position.x, transform.position.y);
        rb.rotation = transform.rotation.z;

        // Move the Spell with the given speed
        rb.AddRelativeForce(direction * spellData.Speed, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Deletes the Object when the lifetime ends
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeleteTimer()
    {
        yield return new WaitForSeconds(spellData.Lifetime);
        Delete();
    }

    /// <summary>
    /// Deals damage on Collision with an Enemy
    /// </summary>
    /// <param name="_collision"></param>
    private void OnCollisionEnter(Collision _collision)
    {
        // if an enemy got hit by the spell
        if (_collision.gameObject.CompareTag("Enemy"))
        {
            _collision.gameObject.TryGetComponent(out IDamagable character);
            character.GetDamage(spellData.Damage);

            health -= 1;
            if (health <= 0) Delete();
        }
    }
}
