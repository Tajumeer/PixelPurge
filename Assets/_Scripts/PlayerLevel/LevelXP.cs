using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelXP : MonoBehaviour
{
    [SerializeField] private Transform playerTransf;
    private Rigidbody2D rb;

    [SerializeField] private float speed;
    private bool isCollected = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        isCollected = false;
    }

    public void CollectXP()
    {
        isCollected = true;
    }

    private void Update()
    {
        if(isCollected)
            rb.AddRelativeForce(playerTransf.position * speed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (!_collision.gameObject.CompareTag("Player")) return;

        _collision.gameObject.TryGetComponent(out LevelPlayer character);
        character.GetXP();
    }
}
