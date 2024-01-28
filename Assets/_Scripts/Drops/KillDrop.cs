using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KillDrop : MonoBehaviour
{
    private PlayerController m_player;

    [SerializeField] private float m_speed;

    private bool m_isCollected;

    private void Awake()
    {
        m_player = FindObjectOfType<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        // if the kill drop reaches the player, it is collected and kills all enemies
        if (_collision.gameObject.CompareTag("Player"))
        {
            Kill();
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

    private void Kill()
    {
        GameObject[] enemyObj = GameObject.FindGameObjectsWithTag("Enemy");
        
       foreach (GameObject enemy in enemyObj)
        {
            if (enemy.GetComponent<FrostGhostController>())
            {
                FrostGhostController ghostController = enemy.GetComponent<FrostGhostController>();
                ghostController.SetDeathState();

            }
            else if (enemy.GetComponent<FrostGolemController>())
            {
                FrostGolemController golemController = enemy.GetComponent<FrostGolemController>();
                golemController.SetDeathState();
            }
            else if (enemy.GetComponent<RangedController>())
            {
                RangedController rangedController = enemy.GetComponent<RangedController>();
                rangedController.SetDeathState();
            }
        }
    }
}
