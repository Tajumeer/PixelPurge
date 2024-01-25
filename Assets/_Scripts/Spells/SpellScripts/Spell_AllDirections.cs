using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class Spell_AllDirections : PoolObject<Spell_AllDirections>
{
    private Rigidbody2D m_rb;
    private SO_AllDirections m_spellData;

    private float m_health;

    /// <summary>
    /// Get & reset Rigidbody, 
    /// start Lifetime & DeleteTimer,
    /// move
    /// </summary>
    /// <param name="_spellIdx"></param>
    public void OnSpawn(SO_AllDirections _spellData, int _spellIdx)
    {
        InitRigidbody();

        m_spellData = _spellData;

        // Start Lifetime
        StartCoroutine(DeleteTimer());
        m_health = m_spellData.EnemyHitPoints;

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

        m_rb.AddRelativeForce(direction * m_spellData.Speed, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Get and reset rigidbody
    /// </summary>
    private void InitRigidbody()
    {
        if (m_rb == null) m_rb = GetComponent<Rigidbody2D>();
        m_rb.velocity = new Vector2(0f, 0f);
        m_rb.position = new Vector2(transform.position.x, transform.position.y);
        m_rb.rotation = transform.localRotation.z;
    }

    public void OnTriggerEnter2D(Collider2D _collision)
    {
        // only an enemy can get hit by the spell
        if (!_collision.gameObject.CompareTag("Enemy")) return;

        // the enemy get damage on hit
        _collision.gameObject.GetComponent<IDamagable>().GetDamage(m_spellData.Damage);

        // and the spell loses duration or dies
        m_health -= 1;
        if (m_health <= 0) DeactivateSpell();
    }

    /// <summary>
    /// Deletes the Object when the lifetime ends
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeleteTimer()
    {
        yield return new WaitForSeconds(m_spellData.Lifetime);

        DeactivateSpell();
    }

    private void DeactivateSpell()
    {
        StopAllCoroutines();

        Deactivate();
    }
}
