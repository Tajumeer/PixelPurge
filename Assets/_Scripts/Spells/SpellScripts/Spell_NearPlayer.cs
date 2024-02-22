using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class Spell_NearPlayer : PoolObject<Spell_NearPlayer>
{
    private Rigidbody2D m_rb;
    private SO_ActiveSpells m_spellData;
    private PlayerStats m_playerData;

    /// <summary>
    /// Get & reset Rigidbody, 
    /// start Lifetime & DeleteTimer,
    /// move
    /// </summary>
    /// <param name="_spellIdx"></param>
    public void OnSpawn(PlayerStats _playerData, SO_ActiveSpells _spellData)
    {
        InitRigidbody();

        m_spellData = _spellData;
        m_playerData = _playerData;

        // set Radius depending on own radius and player multiplier
        if (m_spellData.Radius.Length >= m_spellData.Level)
        {
            float radius = m_spellData.Radius[m_spellData.Level - 1] * m_playerData.AreaMultiplier;
            transform.localScale = new Vector3(radius, radius, radius);
        }
        else
            transform.localScale = new Vector3(
                transform.localScale.x * m_playerData.AreaMultiplier,
                transform.localScale.y * m_playerData.AreaMultiplier,
                transform.localScale.z * m_playerData.AreaMultiplier);

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
        m_rb.AddRelativeForce(Vector2.down * m_spellData.Speed[m_spellData.Level-1], ForceMode2D.Impulse);
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

        // Calculate Damage
        float damage = m_spellData.Damage[m_spellData.Level - 1];       // the damage of the spell
        damage *= m_playerData.DamageMultiplier;                        // + the damage of the player
        if (Random.Range(1, 101) <= m_playerData.CritChance * 100)      // if it crits
            damage *= m_playerData.CritMultiplier;                      // + crit damage

        // the enemy get damage on hit
        _collision.gameObject.GetComponent<IDamagable>().GetDamage(damage);
    }

    /// <summary>
    /// Deletes the Object when the lifetime ends
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeleteTimer()
    {
        yield return new WaitForSeconds(m_spellData.Lifetime[m_spellData.Level-1]);

        DeactivateSpell();
    }

    private void DeactivateSpell()
    {
        StopAllCoroutines();

        Deactivate();
    }
}
