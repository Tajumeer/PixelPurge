using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float m_movementSpeed;

    [Header("Dashing")]
    [SerializeField] private float m_dashingPower;
    [SerializeField] private float m_dashinTime;
    [SerializeField] private float m_dashingCooldown;
    private bool m_isAbleToDash;
    private bool m_isDashing;


    [Header("Components")]
    [SerializeField] private SpriteRenderer m_spriteRenderer;

    private Vector2 m_moveDirection;
    private Rigidbody2D m_rigidbody;


    void Start()
    {
        m_isAbleToDash = true;
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (m_isDashing) { return; }

        Inputs();

        if (Input.GetKey(KeyCode.Space) && m_isAbleToDash)
        {
            StartCoroutine(Dash());
        }
    }
    private void FixedUpdate()
    {
        if (m_isDashing) { return; }
        Move();
    }

    private void Inputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        m_moveDirection = new Vector2(moveX, moveY).normalized;

        if (m_moveDirection.x == -1f) { m_spriteRenderer.flipX = true; }
        else if (m_moveDirection.x == 1f) { m_spriteRenderer.flipX = false; }
    }

    private void Move()
    {
        m_rigidbody.velocity = new Vector2(m_moveDirection.x * m_movementSpeed, m_moveDirection.y * m_movementSpeed);
    }

    private IEnumerator Dash()
    {
        m_isAbleToDash = false;
        m_isDashing = true;

        m_rigidbody.velocity = new Vector2(m_moveDirection.x * m_dashingPower, m_moveDirection.y * m_dashingPower);
        yield return new WaitForSeconds(m_dashinTime);

        m_isDashing = false;

        yield return new WaitForSeconds(m_dashingCooldown);
        m_isAbleToDash = true;
    }
}
