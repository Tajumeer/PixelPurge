using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class Spell_PoisonArea : PoolObject<Spell_PoisonArea>
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
    public void OnSpawn(PlayerStats _playerData, SO_ActiveSpells _spellData, Transform _playerTransf)
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

        // Start Lifetime
        StartCoroutine(DeleteTimer());

        SpawnAtRandomPosition(_playerTransf);
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

    /// <summary>
    /// Take a position of an enemy or a random position in the camera view to spawn it
    /// </summary>
    /// <param name="_playerTransf"></param>
    private void SpawnAtRandomPosition(Transform _playerTransf)
    {
        // set Radius
        transform.localScale = new Vector3
            (m_spellData.Radius[m_spellData.Level - 1], m_spellData.Radius[m_spellData.Level - 1], m_spellData.Radius[m_spellData.Level - 1]);

        float radiusToSpawn = FindObjectOfType<Camera>().orthographicSize;      // can spawn within the bounds of the camera
        radiusToSpawn -= (m_spellData.Radius[m_spellData.Level - 1] / 2);       //and with a little space to the bounds

        // randomize position in camera view
        float xPos = Random.Range(-radiusToSpawn, radiusToSpawn);
        float yPos = Random.Range(-radiusToSpawn, radiusToSpawn);
        Vector3 positionToSpawn = new Vector3(_playerTransf.position.x + xPos, _playerTransf.position.y + yPos, _playerTransf.position.z);

        // search for enemys
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemys.Length; i++)
        {
            if (enemys[i].GetComponent<DeathBool>().IsDead) continue;

            // if enemy is in the camera view, instead spawn it on the enemy
            if (enemys[i].transform.position.x > _playerTransf.position.x - radiusToSpawn &&
                enemys[i].transform.position.x < _playerTransf.position.x + radiusToSpawn &&
                enemys[i].transform.position.y > _playerTransf.position.y - radiusToSpawn &&
                enemys[i].transform.position.y < _playerTransf.position.y + radiusToSpawn)
                positionToSpawn = enemys[i].transform.position;
        }

        transform.position = positionToSpawn;
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

        foreach (IDamagable enemy in m_enemysInAura)
        {
            // Calculate Damage
            float damage = m_spellData.Damage[m_spellData.Level - 1];       // the damage of the spell
            damage *= m_playerData.DamageMultiplier;                        // + the damage of the player
            if (Random.Range(1, 101) <= m_playerData.CritChance * 100)      // if it crits
                damage *= m_playerData.CritMultiplier;                      // + crit damage

            enemy.GetDamage(damage);
        }
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
