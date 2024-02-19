using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Bomb : PoolObject<Spell_Bomb>
{
    private Rigidbody2D m_rb;
    private SO_ActiveSpells m_spellData;

    /// <summary>
    /// Get & reset Rigidbody, 
    /// start Lifetime & DeleteTimer,
    /// move
    /// </summary>
    /// <param name="_spellIdx"></param>
    public void OnSpawn(SO_ActiveSpells _spellData, Transform _playerTransf)
    {
        InitRigidbody();

        m_spellData = _spellData;

        // Start Lifetime
        StartCoroutine(DeleteTimer());

        SpawnAtRandomPosition(_playerTransf);
    }

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

        transform.position = positionToSpawn;
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

        Deactivate();
    }
}
