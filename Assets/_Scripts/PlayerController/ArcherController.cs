using System.Collections;
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

    [Header("Basic Attack")]
    [SerializeField] private Transform m_projectileParent;
    [SerializeField] private GameObject m_attackPrefab;
    [SerializeField] private float m_attackForce;
    [SerializeField] private float m_attackSpeed;
    [SerializeField] private float m_attackOffest;
    private float m_attackTimer;
    private Vector2 m_attackDirection;

    [Header("Components")]
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private Animator m_animator;
    private Vector2 m_moveDirection;
    private Rigidbody2D m_rigidbody;

    private bool m_isDead;

    private enum AnimationState
    {
        archer_run,
        archer_idle,
        archer_death,
    }

    private AnimationState m_currentState;


    void Start()
    {
        m_isAbleToDash = true;
        m_isDead = false;
        m_attackTimer = 0;
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (m_attackTimer <= 0f)
        {
            Fire();
            m_attackTimer = m_attackSpeed;
        }
        else m_attackTimer -= Time.deltaTime;

        if (!m_isDead)
        {
            SetAnimation();
        }

        if (m_isDashing) { return; }

        Inputs();

        if (Input.GetKey(KeyCode.Space) && m_isAbleToDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void SetAnimation()
    {
        //TODO:Placeholder function to test death anmiation
        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_isDead = true;
            ChangeAnimationState(AnimationState.archer_death);
            return;
        }
        if (m_moveDirection.x == 0 && m_moveDirection.y == 0) ChangeAnimationState(AnimationState.archer_idle);
        else ChangeAnimationState(AnimationState.archer_run);
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
        m_attackDirection = new Vector2(moveX, moveY).normalized;

        if (m_moveDirection.x < 0f) { m_spriteRenderer.flipX = true; }
        else if (m_moveDirection.x > 0f) { m_spriteRenderer.flipX = false; }
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

    private void Fire()
    {
        GameObject arrow = Instantiate(m_attackPrefab, new Vector3(transform.position.x, transform.position.y - m_attackOffest, 0f), Quaternion.identity, m_projectileParent);
        if (m_attackDirection.x == 0 && m_attackDirection.y == 0)
        {
            if (m_spriteRenderer.flipX == true)
            {
                arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(-1f * m_attackForce, 0f * m_attackForce);
                arrow.GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (m_spriteRenderer.flipX == false)
            {
                arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(1f * m_attackForce, 0f * m_attackForce);
                arrow.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
        else
        {
            arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(m_attackDirection.x * m_attackForce, m_attackDirection.y * m_attackForce);
            //if (arrow.GetComponent<Rigidbody2D>().velocity.x == -1)
            //{
            //    arrow.GetComponent<SpriteRenderer>().flipX = true;
            //}
            //else if(arrow.GetComponent<Rigidbody2D>().velocity.x == 1)
            //{
            //    arrow.GetComponent<SpriteRenderer>().flipX = false;
            //}
        }

        arrow.transform.Rotate(0f, 0f, Mathf.Atan2(m_attackDirection.y, m_attackDirection.x) * Mathf.Rad2Deg);
    }

    void ChangeAnimationState(AnimationState newState)
    {
        string animationName = string.Empty;
        if (m_currentState == newState) return;
        switch (newState)
        {
            case AnimationState.archer_idle:
                animationName = "archer_idle";
                break;
            case AnimationState.archer_run:
                animationName = "archer_run";
                break;
            case AnimationState.archer_death:
                animationName = "archer_death";
                break;
        }
        m_animator.Play(animationName);
        m_currentState = newState;
    }
}
