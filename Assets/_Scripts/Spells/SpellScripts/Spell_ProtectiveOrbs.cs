using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Maya, Sven

public class Spell_ProtectiveOrbs : PoolObject<Spell_ProtectiveOrbs>
{
    private Rigidbody2D m_rb;
    private SO_ActiveSpells m_spellData;
    private float m_angleOffset;
    private int m_orbIndex;
    private PlayerController m_player;
    /// <summary>
    /// Get & reset Rigidbody, 
    /// start Lifetime & DeleteTimer,
    /// move
    /// </summary>
    /// <param name="_spellIdx"></param>
    public void OnSpawn(SO_ActiveSpells _spellData, int _orbIndex)
    {
        InitRigidbody();

        m_spellData = _spellData;
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


        //        float angle = m_orbIndex * m_angleOffset;
        //        Vector2 spawnPosition = new Vector2(
        //            transform.position.x + m_spellData.Radius[m_spellData.Level - 1] * Mathf.Cos(angle * Mathf.Deg2Rad),
        //            transform.position.y + m_spellData.Radius[m_spellData.Level - 1] * Mathf.Sin(angle * Mathf.Deg2Rad)
        //        );

        //       this.transform.position = spawnPosition;
        //}
    }
    private void Update()
    {
        float angle = (m_orbIndex * m_angleOffset) + (Time.time * m_spellData.Speed[m_spellData.Level - 1]);

            Vector2 spawnPosition = new Vector2(
               (m_spellData.Radius[m_spellData.Level - 1] * Mathf.Cos(angle * Mathf.Deg2Rad) - 0.25f),
               m_spellData.Radius[m_spellData.Level - 1] * Mathf.Sin(angle * Mathf.Deg2Rad)
           );

            this.transform.position = (Vector2)m_player.transform.position + spawnPosition;
    }

    public void OnTriggerEnter2D(Collider2D _collision)
    {
        // only an enemy can get hit by the spell
        if (!_collision.gameObject.CompareTag("Enemy")) return;

        // the enemy get damage on hit
        _collision.gameObject.GetComponent<IDamagable>().GetDamage(m_spellData.Damage[m_spellData.Level - 1]);
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
