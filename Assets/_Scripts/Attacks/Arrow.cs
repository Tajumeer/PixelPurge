using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //TODO: Check enemy hit deal damage
   
        if(collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        else
        {
        Destroy(gameObject);

        }
    }
}
