using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class Spell_Aura : MonoBehaviour
{
    private Rigidbody2D m_rb;
    private SO_ActiveSpells m_spellData;

    private float m_activeCD = 0f;
    private Queue<IDamagable> m_enemysInAura;

    /// <summary>
    /// Get & reset Rigidbody, 
    /// start Lifetime & DeleteTimer,
    /// move
    /// </summary>
    /// <param name="_spellIdx"></param>
    public void OnSpawn(SO_ActiveSpells _spellData)
    {
        InitRigidbody();

        m_enemysInAura = new Queue<IDamagable>();

        m_spellData = _spellData;
    }

    private void Update()
    {
        m_activeCD += Time.deltaTime;
        if (m_activeCD >= m_spellData.Cd[m_spellData.Level - 1])
        {
            DealDamage();
            m_activeCD = 0;
        }

        //transform.position = Vector3.zero;
    }

    private void DealDamage()
    {
        if (!m_enemysInAura.TryPeek(out IDamagable temp)) return;

        foreach(IDamagable enemy in m_enemysInAura)
        {
            enemy.GetDamage(m_spellData.Damage[m_spellData.Level - 1]);
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

        // the enemy get damage on hit
        //_collision.gameObject.GetComponent<IDamagable>().GetDamage(spellData.Damage);
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        // only an enemy can get hit by the spell
        if (!_collision.gameObject.CompareTag("Enemy")) return;

        IDamagable enemy = _collision.gameObject.GetComponent<IDamagable>();
        m_enemysInAura.TryDequeue(out enemy);
    }
}
