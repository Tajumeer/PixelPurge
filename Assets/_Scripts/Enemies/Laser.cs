using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    private float m_damageTimer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<IDamagable>().GetDamage(GetComponentInParent<EyeController>().EnemyDamage);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
     
        
        if (collision.gameObject.CompareTag("Player"))
        {
            m_damageTimer += Time.deltaTime;

            if(m_damageTimer < 0.5f) {  return; }

            collision.gameObject.GetComponent<IDamagable>().GetDamage(GetComponentInParent<EyeController>().EnemyDamage);
            m_damageTimer = 0f;
        }
    }
}
