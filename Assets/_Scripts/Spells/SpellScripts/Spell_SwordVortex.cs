using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Maya, Sven

public class Spell_SwordVortex : PoolObject<Spell_SwordVortex>
{
    private Rigidbody2D m_rb;
    private SO_ActiveSpells m_spellData;
    private PlayerStats m_playerData;
    private float m_angleOffset;
    private int m_orbIndex;
    private PlayerController m_player;
    /// <summary>
    /// Get & reset Rigidbody, 
    /// start Lifetime & DeleteTimer,
    /// move
    /// </summary>
    /// <param name="_spellIdx"></param>
    public void OnSpawn(PlayerStats _playerData, SO_ActiveSpells _spellData, int _orbIndex)
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

        // Start Lifetime
        StartCoroutine(DeleteTimer());

        // AB HIER KANNST DU WAS MACHEN
        m_orbIndex = _orbIndex;
        m_player = FindObjectOfType<PlayerController>();
        Shoot();
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

    private void Shoot()
    {
        m_angleOffset = 360f / m_spellData.ProjectileAmount[m_spellData.Level - 1];

        Vector2 initialDirection = m_player.transform.position - this.transform.position;
        float initialRotationAngle = Mathf.Atan2(initialDirection.y, initialDirection.x) * Mathf.Rad2Deg;

        this.transform.rotation = Quaternion.Euler(0, 0, initialRotationAngle);
    }
    private void Update()
    {
        float angle = (m_orbIndex * m_angleOffset) + (Time.time * m_spellData.Speed[m_spellData.Level - 1]);

            Vector2 spawnPosition = new Vector2(
               (m_spellData.Radius[m_spellData.Level - 1] * Mathf.Cos(angle * Mathf.Deg2Rad) - 0.25f),
               m_spellData.Radius[m_spellData.Level - 1] * Mathf.Sin(angle * Mathf.Deg2Rad)
           );

            this.transform.position = (Vector2)m_player.transform.position + spawnPosition;

        Vector2 direction = m_player.transform.position - this.transform.position;
        float rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the rotation to the projectile
        this.transform.rotation = Quaternion.Euler(0, 0, rotationAngle + 180f);
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
        yield return new WaitForSeconds(m_spellData.Lifetime[m_spellData.Level - 1]);

        DeactivateSpell();
    }

    private void DeactivateSpell()
    {
        StopAllCoroutines();

        Deactivate();
    }
}
