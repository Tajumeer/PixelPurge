using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya, Sven

public class Spell_AirWave : PoolObject<Spell_AirWave>
{
    private Rigidbody2D m_rb;
    private SO_ActiveSpells m_spellData;
    private PlayerStats m_playerData;
    [SerializeField] private Transform m_pivot;
    private float m_health;

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

        // Start Lifetime
        StartCoroutine(DeleteTimer());
        m_health = m_spellData.Pierce[m_spellData.Level - 1];

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

        float angle = Mathf.Atan2(arrowDirection.y, arrowDirection.x) * Mathf.Rad2Deg;


        Vector2 direction = new Vector2(arrowDirection.x, arrowDirection.y).normalized;
        m_rb.AddRelativeForce(direction * m_spellData.Speed[m_spellData.Level - 1], ForceMode2D.Impulse);

        m_pivot.rotation = Quaternion.Euler(0, 0, angle - 90f);
       // transform.Rotate(0f, 0f, Mathf.Atan2(arrowDirection.y, arrowDirection.x) * Mathf.Rad2Deg);
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
        yield return new WaitForSeconds(m_spellData.Lifetime[m_spellData.Level - 1]);

        DeactivateSpell();
    }

    private void DeactivateSpell()
    {
        StopAllCoroutines();

        Deactivate();
    }
}
