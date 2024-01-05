using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{
    [Header("Combat Stats")]
    [SerializeField] public float PlayerHealth;
    [SerializeField] public float PlayerDamage;

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
    //private Vector2 m_attackDirection;
    //private const float F_ATTACK_OFFSET_X = 0.25f;

    [Header("Components")]
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private Animator m_animator;
    //[HideInInspector]
    public Vector2 MoveDirection;
    private Rigidbody2D m_rigidbody;

    private bool m_isDead;

    public enum AnimationState
    {
        archer_run,
        archer_idle,
        player_death,
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

        if (PlayerHealth <= 0)
        {
            SetDeathState();

        }

        if (m_attackTimer <= 0f && !m_isDead)
        {
            Fire();
            m_attackTimer = m_attackSpeed;
        }
        else m_attackTimer -= Time.deltaTime;


        SetAnimation();

        if (m_isDead) { return; }
        if (m_isDashing) { return; }

        Inputs();

        if (Input.GetKey(KeyCode.Space) && m_isAbleToDash)
        {
            StartCoroutine(Dash());
        }

    }

    private void SetDeathState()
    {
        m_isDead = true;
        m_spriteRenderer.sortingOrder = 0;
        MoveDirection = Vector3.zero;
        //Time.timeScale = 0f;
    }

    private void SetAnimation()
    {
        if (m_isDead)
        {
            ChangeAnimationState(AnimationState.player_death);
            return;
        }
        if (MoveDirection.x == 0 && MoveDirection.y == 0) ChangeAnimationState(AnimationState.archer_idle);
        else ChangeAnimationState(AnimationState.archer_run);
    }

    private void FixedUpdate()
    {
        Debug.Log(PlayerHealth + "|" + m_isDead);
        if (m_isDashing) { return; }
        Move();
    }

    private void Inputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        MoveDirection = new Vector2(moveX, moveY).normalized;
        //m_attackDirection = new Vector2(moveX, moveY).normalized;

        if (MoveDirection.x < 0f) { m_spriteRenderer.flipX = true; }
        else if (MoveDirection.x > 0f) { m_spriteRenderer.flipX = false; }
    }

    private void Move()
    {
        m_rigidbody.velocity = new Vector2(MoveDirection.x * m_movementSpeed, MoveDirection.y * m_movementSpeed);
    }

    private IEnumerator Dash()
    {
        m_isAbleToDash = false;
        m_isDashing = true;

        m_rigidbody.velocity = new Vector2(MoveDirection.x * m_dashingPower, MoveDirection.y * m_dashingPower);
        yield return new WaitForSeconds(m_dashinTime);

        m_isDashing = false;

        yield return new WaitForSeconds(m_dashingCooldown);
        m_isAbleToDash = true;
    }

    private void Fire()
    {
        //Fire in walk direction
        //
        //GameObject arrow;
        //if (m_spriteRenderer.flipX == true && m_attackDirection.y ==0)
        //{
        //    arrow = Instantiate(m_attackPrefab, new Vector3(transform.position.x - F_ATTACK_OFFSET_X, transform.position.y - m_attackOffest, 0f), Quaternion.identity, m_projectileParent);
        //}
        //else if (m_spriteRenderer.flipX == false && m_attackDirection.y == 0)
        //{
        //    arrow = Instantiate(m_attackPrefab, new Vector3(transform.position.x + F_ATTACK_OFFSET_X, transform.position.y - m_attackOffest, 0f), Quaternion.identity, m_projectileParent);
        //}
        //else
        //{
        //    arrow = Instantiate(m_attackPrefab, new Vector3(transform.position.x, transform.position.y - m_attackOffest, 0f), Quaternion.identity, m_projectileParent);
        //}

        //if (m_attackDirection.x == 0 && m_attackDirection.y == 0)
        //{
        //    if (m_spriteRenderer.flipX == true)
        //    {
        //        arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(-1f * m_attackForce, 0f * m_attackForce);
        //        arrow.GetComponent<SpriteRenderer>().flipX = true;
        //    }
        //    else if (m_spriteRenderer.flipX == false)
        //    {
        //        arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(1f * m_attackForce, 0f * m_attackForce);
        //        arrow.GetComponent<SpriteRenderer>().flipX = false;
        //    }
        //}
        //else
        //{
        //    arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(m_attackDirection.x * m_attackForce, m_attackDirection.y * m_attackForce);
        //}

        //arrow.transform.Rotate(0f, 0f, Mathf.Atan2(m_attackDirection.y, m_attackDirection.x) * Mathf.Rad2Deg);


        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 arrowDirection = (mousePosition - transform.position).normalized;

        GameObject arrow = Instantiate(m_attackPrefab, new Vector3(transform.position.x, transform.position.y - m_attackOffest, 0f), Quaternion.identity, m_projectileParent);

        arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(arrowDirection.x * m_attackForce, arrowDirection.y * m_attackForce);
        arrow.transform.Rotate(0f, 0f, Mathf.Atan2(arrowDirection.y, arrowDirection.x) * Mathf.Rad2Deg);

    }

    public void ChangeAnimationState(AnimationState newState)
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
            case AnimationState.player_death:
                animationName = "archer_death";
                break;
        }
        m_animator.Play(animationName);
        m_currentState = newState;
    }

    public void GetDamage(float _damageValue)
    {
        this.PlayerHealth -= _damageValue;
    }
}
