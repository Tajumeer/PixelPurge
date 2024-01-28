using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealDrop : MonoBehaviour
{

    private PlayerController m_player;

    [SerializeField] private int m_healAmount;
    [SerializeField] private float m_speed;

    private bool m_isCollected;

    private void Awake()
    {
        m_player = FindObjectOfType<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        // if the heal reaches the player, it is collected and heals the player
        if (_collision.gameObject.CompareTag("Player"))
        {
            Heal();
              Destroy(this.gameObject);
        }

        // if the player collects the xp (when it is in the collection radius) it flies to him
        else if (_collision.gameObject.CompareTag("PlayerXpCollect"))
        {
            m_isCollected = true;
        }
    }

    private void FixedUpdate()
    {
        // Move towards player if he collected the Heal
        if (m_isCollected)
            gameObject.transform.position = Vector2.MoveTowards(transform.position, m_player.gameObject.transform.position, m_speed * Time.deltaTime);
    }

    private void Heal()
    {
        m_player.CurrentHealth += m_healAmount;
        if (m_player.CurrentHealth > m_player.MaxHealth)
        {
            m_player.CurrentHealth = m_player.MaxHealth;
        }
    }
}
