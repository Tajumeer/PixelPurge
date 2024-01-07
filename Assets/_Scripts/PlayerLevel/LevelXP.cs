using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelXP : MonoBehaviour
{
    [SerializeField] private Transform playerTransf;
    private Rigidbody2D rb;

    [SerializeField] private float speed;
    private float xpAmount;
    private bool isCollected;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(isCollected)
            rb.AddRelativeForce(playerTransf.position * speed, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Spawn the XP with a given xp amount in it
    /// </summary>
    /// <param name="_xpAmount">The amount of XP the player gets when collected</param>
    public void OnSpawn(float _xpAmount)
    {
        isCollected = false;
        xpAmount = _xpAmount;
    }

    /// <summary>
    /// Is the XP "Collected" / is it in the collection radius of the player -> then fly to him 
    /// </summary>
    public void CollectXP()
    {
        isCollected = true;
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        // if the xp reaches the player, it is collected and increase XP
        if (_collision.gameObject.CompareTag("Player"))
        {
            _collision.gameObject.TryGetComponent(out LevelPlayer character);
            character.GetXP(xpAmount);
        }

        // if the player collects the xp (when it is in the collection radius) it flies to him
        else if (_collision.gameObject.CompareTag("PlayerXpCollect"))
        {
            CollectXP();
        }
    }
}
