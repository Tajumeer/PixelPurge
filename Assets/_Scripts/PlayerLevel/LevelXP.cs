using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class LevelXP : PoolObject<LevelXP>
{
    private PlayerController player;
    private LevelPlayer levelScript;

    [SerializeField] private float speed;
    private float xpAmount;
    private bool isCollected;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();

        // Debugging
        isCollected = false;
        xpAmount = 1f;
    }

    /// <summary>
    /// Spawn the XP with a given xp amount in it
    /// </summary>
    /// <param name="_xpAmount">The amount of XP the player gets when collected</param>
    public void OnSpawn(float _xpAmount, LevelPlayer _levelScript)
    {
        isCollected = false;
        xpAmount = _xpAmount;
        levelScript = _levelScript;
    }

    private void FixedUpdate()
    {
        // Move towards player if he collected the xp
        if (isCollected)
            gameObject.transform.position = Vector2.MoveTowards(transform.position, player.gameObject.transform.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        // if the xp reaches the player, it is collected and increase XP
        if (_collision.gameObject.CompareTag("Player"))
        {
            levelScript.GetXP(xpAmount);
            Deactivate();
        }

        // if the player collects the xp (when it is in the collection radius) it flies to him
        else if (_collision.gameObject.CompareTag("PlayerXpCollect"))
        {
            isCollected = true;
        }
    }
}
