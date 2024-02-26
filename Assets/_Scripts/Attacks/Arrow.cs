using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float m_projectileLifetime;

    private void Update()
    {
        m_projectileLifetime -= Time.deltaTime;

        if (m_projectileLifetime <= 0)
        {
            Destroy(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<IDamagable>().GetDamage(GetComponentInParent<RangedController>().EnemyDamage);
            Destroy(this.gameObject);
        }
    }
}
