using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// Maya, Sven

public class Spell_NewSpell1 : PoolObject<Spell_NewSpell1>
{
    private Rigidbody2D m_rb;
    private SO_ActiveSpells m_spellData;
    private enum States { Shooting, Returning }
    private States m_currentState;
    [SerializeField] private float m_travelDistance;
    [SerializeField] private float m_rotationSpeed;
    private Vector2 m_initialPosition;

    private PlayerController m_playerController;

    /// <summary>
    /// Get & reset Rigidbody, 
    /// start Lifetime & DeleteTimer,
    /// move
    /// </summary>
    /// <param name="_spellIdx"></param>
    public void OnSpawn(SO_ActiveSpells _spellData, int _objIndex)
    {
        InitRigidbody();
        m_spellData = _spellData;
        m_playerController = FindObjectOfType<PlayerController>();

        // Start Lifetime
       // StartCoroutine(DeleteTimer());

        m_initialPosition = m_rb.position;
        m_currentState = States.Shooting;
        Shoot();
    }

    private void Update()
    {
        switch (m_currentState)
        {
            case States.Shooting:
                ShootingState();
                break;
            case States.Returning:
                ReturningState();
                break;
        }
    }

    public void Shoot()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 projectileDirection = (mousePosition - transform.position).normalized;

        Vector2 direction = new Vector2(projectileDirection.x, projectileDirection.y).normalized;

        m_rb.AddForce(direction * m_spellData.Speed[m_spellData.Level - 1], ForceMode2D.Impulse);
    }

    private void ShootingState()
    {
        if (Vector2.Distance(m_initialPosition, m_rb.position) >= m_travelDistance)
        {
            m_currentState = States.Returning;
        }

        RotateBoomerang();
    }

    private void ReturningState()
    {
        Vector2 returnDirection = ((Vector2)m_playerController.transform.position - m_rb.position).normalized;

        float remainingDistance = Vector2.Distance((Vector2)m_playerController.transform.position, m_rb.position);

        float accelerationFactor = 2f;

        float returnSpeed = m_spellData.Speed[m_spellData.Level - 1] + accelerationFactor / remainingDistance;

        m_rb.velocity = returnDirection * returnSpeed;

        if (Vector2.Distance((Vector2)m_playerController.transform.position, m_rb.position) <= .5f)
        {
           DeactivateSpell();
        }

        RotateBoomerang();
    }

    private void RotateBoomerang()
    {
        float rotationAmount = m_rotationSpeed * Time.deltaTime;
        m_rb.MoveRotation(m_rb.rotation + rotationAmount);
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
