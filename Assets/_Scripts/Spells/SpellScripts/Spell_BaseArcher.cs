using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya, Sven

public class Spell_BaseArcher : PoolObject<Spell_BaseArcher>
{
    private Rigidbody2D m_rb;
    private SO_BaseArcher m_spellData;

    private float m_health;

    /// <summary>
    /// Get & reset Rigidbody, 
    /// start Lifetime & DeleteTimer,
    /// move
    /// </summary>
    /// <param name="_spellIdx"></param>
    public void OnSpawn(SO_BaseArcher _spellData)
    {
        InitRigidbody();

        m_spellData = _spellData;

        // Start Lifetime
        StartCoroutine(DeleteTimer());
        m_health = m_spellData.EnemyHitPoints;

        /**** Sven Start ****/

        if (m_spellData.SpellSFX != null)
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
        m_rb.AddRelativeForce(direction * m_spellData.Speed, ForceMode2D.Impulse);

        transform.Rotate(0f, 0f, Mathf.Atan2(arrowDirection.y, arrowDirection.x) * Mathf.Rad2Deg);
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
