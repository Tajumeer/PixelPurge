using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// Maya

public class Spell_HomingRock : PoolObject<Spell_HomingRock>
{
    private Rigidbody2D m_rb;
    private SO_ActiveSpells m_spellData;
    private PlayerStats m_playerData;
    private Transform m_target;
    [SerializeField] private float m_rotationSpeed;
    private SpriteRenderer m_spriteRenderer;
    private CircleCollider2D m_circleCollider;
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
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_circleCollider = GetComponent<CircleCollider2D>();
        m_spriteRenderer.enabled = false;
        m_circleCollider.enabled = false;
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

        SetTarget();
    }

    private void Update()
    {
        if (m_target != null)
        {
            m_spriteRenderer.enabled = true;
            m_circleCollider.enabled = true;

            if (ValidateTarget())
            {
                Vector2 direction = (Vector2)m_target.position - (Vector2)transform.position;
                direction.Normalize();

                float rotateAmount = Vector3.Cross(direction, transform.up).z;
                GetComponent<Rigidbody2D>().angularVelocity = -rotateAmount * m_rotationSpeed;

                transform.Translate(direction * m_spellData.Speed[m_spellData.Level - 1] * Time.deltaTime);
            }
            else
            {
                SetTarget();
            }
        }
        else if (m_target == null)
        {
            m_spriteRenderer.enabled = false;
            m_circleCollider.enabled = false;
        }

        if(m_target.GetComponent<DeathBool>().IsDead == true)
        {
            DeactivateSpell();
        }
    }

    private bool ValidateTarget()
    {
        if (m_target.GetComponent<DeathBool>().IsDead)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Move away from the player in the given direction for this projecitle
    /// </summary>
    /// <param name="_spellIdx"></param>
    private void SetTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length > 0)
        {
            Transform nearestEnemy = enemies[0].transform;

            foreach (GameObject enemy in enemies)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                float nearestDistance = Vector2.Distance(transform.position, nearestEnemy.position);

                if (distance < nearestDistance)
                {
                    nearestEnemy = enemy.transform;
                }
            }

            m_target = nearestEnemy;
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

        // Calculate Damage
        float damage = m_spellData.Damage[m_spellData.Level - 1];       // the damage of the spell
        damage *= m_playerData.DamageMultiplier;                        // + the damage of the player
        if (Random.Range(1, 101) <= m_playerData.CritChance * 100)      // if it crits
            damage *= m_playerData.CritMultiplier;                      // + crit damage

        // the enemy get damage on hit
        _collision.gameObject.GetComponent<IDamagable>().GetDamage(damage);


        DeactivateSpell();
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
