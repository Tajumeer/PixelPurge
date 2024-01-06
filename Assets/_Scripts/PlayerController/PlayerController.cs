using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{
    public List<PlayerStats> PlayerData;
    public List<GameObject> PlayerVisual;
    [HideInInspector] public PlayerStats ActivePlayerData;
    [HideInInspector] public int ClassIndex; 

    private bool m_isAbleToDash;
    private bool m_isDashing;


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

    /// <summary>
    /// the Int is the index for the List on the PlayerController to choose which Data is to be used
    /// </summary>
    /// <param name="_newIndex"></param>
    public void SetActivePlayerData(int _newIndex)
    {
        if(_newIndex > PlayerData.Count)
        {
            Debug.Log("SetActivePlayerData Error: " + _newIndex + "is out of Bounds check if the new index is correct:: No Changes have been Made to ActivePlayerData");
            return;
        }
        ActivePlayerData = PlayerData[_newIndex];
    }

   private void Start()
    {
        ActivePlayerData = PlayerData[ClassIndex];
        ActivePlayerData.CurrentHealth = ActivePlayerData.MaxHealth;
        m_isAbleToDash = true;
        m_isDead = false;
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (ActivePlayerData.CurrentHealth <= 0)
        {
            SetDeathState();
        }


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
        m_rigidbody.velocity = new Vector2(MoveDirection.x * ActivePlayerData.MovementSpeed, MoveDirection.y * ActivePlayerData.MovementSpeed);
    }

    private IEnumerator Dash()
    {
        m_isAbleToDash = false;
        m_isDashing = true;

        m_rigidbody.velocity = new Vector2(MoveDirection.x * ActivePlayerData.DashSpeed, MoveDirection.y * ActivePlayerData.DashSpeed);
        yield return new WaitForSeconds(ActivePlayerData.DashTime);

        m_isDashing = false;

        yield return new WaitForSeconds(ActivePlayerData.DashCooldown);
        m_isAbleToDash = true;
    }

    //private void Fire()
    //{
    //    //Fire in walk direction
    //    //
    //    //GameObject arrow;
    //    //if (m_spriteRenderer.flipX == true && m_attackDirection.y ==0)
    //    //{
    //    //    arrow = Instantiate(m_attackPrefab, new Vector3(transform.position.x - F_ATTACK_OFFSET_X, transform.position.y - m_attackOffest, 0f), Quaternion.identity, m_projectileParent);
    //    //}
    //    //else if (m_spriteRenderer.flipX == false && m_attackDirection.y == 0)
    //    //{
    //    //    arrow = Instantiate(m_attackPrefab, new Vector3(transform.position.x + F_ATTACK_OFFSET_X, transform.position.y - m_attackOffest, 0f), Quaternion.identity, m_projectileParent);
    //    //}
    //    //else
    //    //{
    //    //    arrow = Instantiate(m_attackPrefab, new Vector3(transform.position.x, transform.position.y - m_attackOffest, 0f), Quaternion.identity, m_projectileParent);
    //    //}

    //    //if (m_attackDirection.x == 0 && m_attackDirection.y == 0)
    //    //{
    //    //    if (m_spriteRenderer.flipX == true)
    //    //    {
    //    //        arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(-1f * m_attackForce, 0f * m_attackForce);
    //    //        arrow.GetComponent<SpriteRenderer>().flipX = true;
    //    //    }
    //    //    else if (m_spriteRenderer.flipX == false)
    //    //    {
    //    //        arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(1f * m_attackForce, 0f * m_attackForce);
    //    //        arrow.GetComponent<SpriteRenderer>().flipX = false;
    //    //    }
    //    //}
    //    //else
    //    //{
    //    //    arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(m_attackDirection.x * m_attackForce, m_attackDirection.y * m_attackForce);
    //    //}

    //    //arrow.transform.Rotate(0f, 0f, Mathf.Atan2(m_attackDirection.y, m_attackDirection.x) * Mathf.Rad2Deg);


    //    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    Vector2 arrowDirection = (mousePosition - transform.position).normalized;

    //    GameObject arrow = Instantiate(m_attackPrefab, new Vector3(transform.position.x, transform.position.y - m_attackOffest, 0f), Quaternion.identity, m_projectileParent);

    //    arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(arrowDirection.x * m_attackForce, arrowDirection.y * m_attackForce);
    //    arrow.transform.Rotate(0f, 0f, Mathf.Atan2(arrowDirection.y, arrowDirection.x) * Mathf.Rad2Deg);

    //}

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
        this.ActivePlayerData.CurrentHealth -= _damageValue;
    }
}
