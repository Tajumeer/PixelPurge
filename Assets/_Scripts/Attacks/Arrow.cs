using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Arrow : MonoBehaviour
{
    private float m_damageToDeal;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //TODO: Check enemy hit deal damage

        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            m_damageToDeal = GameObject.FindWithTag("Player").GetComponent<PlayerController>().PlayerDamage;
            collision.gameObject.GetComponent<IDamagable>().GetDamage(m_damageToDeal);
            Debug.Log("Hiy Enemy with Arrow");
            Destroy(this.gameObject);
        }
    }
}
