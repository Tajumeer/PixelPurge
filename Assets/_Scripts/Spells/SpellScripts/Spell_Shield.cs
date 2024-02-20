using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Shield : PoolObject<Spell_Shield>
{
    [SerializeField] private SO_AllSpells m_data_allSpells;
    [SerializeField] private float m_damageReductionPercentage = 20f;
    private Rigidbody2D m_rb;
    private SO_ActiveSpells m_spellData;

    /// <summary>
    /// Get & reset Rigidbody, 
    /// start Lifetime & DeleteTimer,
    /// move
    /// </summary>
    /// <param name="_spellIdx"></param>
    public void OnSpawn(SO_ActiveSpells _spellData)
    {
        InitRigidbody();

        m_spellData = _spellData;

        m_data_allSpells.statSO.DamageReductionPercentage += m_damageReductionPercentage;

        // Start Lifetime
        StartCoroutine(DeleteTimer());

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

        gameObject.SetActive(false);
    }
}
