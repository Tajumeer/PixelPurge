using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class Spell_Aura : MonoBehaviour
{
    private Rigidbody2D rb;
    private SO_Aura spellData;

    private float activeCD = 0f;
    private Queue<IDamagable> enemysInAura;

    /// <summary>
    /// Get & reset Rigidbody, 
    /// start Lifetime & DeleteTimer,
    /// move
    /// </summary>
    /// <param name="_spellIdx"></param>
    public void OnSpawn(SO_Aura _spellData, int _spellIdx)
    {
        InitRigidbody();

        spellData = _spellData;
    }

    private void Update()
    {
        activeCD += Time.deltaTime;
        if (activeCD >= spellData.Cd)
        {
            DealDamage();
            activeCD = 0;
        }
    }

    private void DealDamage()
    {
        foreach(IDamagable enemy in enemysInAura)
        {
            enemy.GetDamage(spellData.Damage);
        }
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

        enemysInAura.Enqueue(_collision.gameObject.GetComponent<IDamagable>());

        // the enemy get damage on hit
        //_collision.gameObject.GetComponent<IDamagable>().GetDamage(spellData.Damage);
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        // only an enemy can get hit by the spell
        if (!_collision.gameObject.CompareTag("Enemy")) return;

        IDamagable enemy = _collision.gameObject.GetComponent<IDamagable>();
        enemysInAura.TryDequeue(out enemy);
    }
}
