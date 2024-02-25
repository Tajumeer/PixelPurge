using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class Spell_Aura : MonoBehaviour
{
    private Rigidbody2D m_rb;
    private SO_ActiveSpells m_spellData;
    private PlayerStats m_playerData;

    private float m_activeCD = 0f;
    private Queue<IDamagable> m_enemysInAura;

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
        if (m_spellData.Radius.Length == m_spellData.MaxLevel)
        {
            float radius = m_spellData.Radius[m_spellData.Level - 1] * m_playerData.AreaMultiplier;
            transform.localScale = new Vector3(radius, radius, radius);
        }
        else
            transform.localScale = new Vector3(
                transform.localScale.x * m_playerData.AreaMultiplier,
                transform.localScale.y * m_playerData.AreaMultiplier,
                transform.localScale.z * m_playerData.AreaMultiplier);

        m_enemysInAura = new Queue<IDamagable>();

        m_spellData = _spellData;
    }

    private void Update()
    {
        m_activeCD += Time.deltaTime;
        if (m_activeCD >= m_spellData.InternalCd[m_spellData.Level - 1])
        {
            DealDamage();
            m_activeCD = 0;
        }

    }

    private void DealDamage()
    {
        if (!m_enemysInAura.TryPeek(out IDamagable temp)) return;

        foreach(IDamagable enemy in m_enemysInAura)
        {
            // Calculate Damage
            float damage = m_spellData.Damage[m_spellData.Level - 1];       // the damage of the spell
            damage *= m_playerData.DamageMultiplier;                        // + the damage of the player
            if (Random.Range(1, 101) <= m_playerData.CritChance * 100)      // if it crits
                damage *= m_playerData.CritMultiplier;                      // + crit damage

            enemy.GetDamage(damage);
        }
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

        m_enemysInAura.Enqueue(_collision.gameObject.GetComponent<IDamagable>());
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        // only an enemy can get hit by the spell
        if (!_collision.gameObject.CompareTag("Enemy")) return;

        IDamagable enemy = _collision.gameObject.GetComponent<IDamagable>();
        m_enemysInAura.TryDequeue(out enemy);
    }
}
