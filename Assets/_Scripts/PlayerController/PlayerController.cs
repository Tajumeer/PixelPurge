using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamagable
{
    public List<PlayerStats> PlayerData;
    public List<GameObject> PlayerVisual;
    [SerializeField] private AudioClip m_damageTakenClip;
    [HideInInspector] public PlayerStats ActivePlayerData;
    [HideInInspector] public int ClassIndex;

    private int m_characterIndex;

    private bool m_isAbleToDash;
    private bool m_isDashing;

    [SerializeField] private Image m_healthBar;

    [Header("Components")]
    // [SerializeField] private SpriteRenderer m_spriteRenderer;
    // [SerializeField] private Animator m_animator;
    [HideInInspector] public Vector2 MoveDirection;
    private Rigidbody2D m_rigidbody;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;

    #region PlayerStats
    //   [Header("Player Stats")]
    //[HideInInspector] public string PlayerClassName;
    //[HideInInspector] public int Level;
    //[HideInInspector] public float MaxHealth;
    //[HideInInspector] public float CurrentHealth;
    //[HideInInspector] public float MovementSpeed;
    //[HideInInspector] public float DashAmount;
    //[HideInInspector] public float DashSpeed;
    //[HideInInspector] public float DashCooldown;
    //[HideInInspector] public float DashTime;
    //[HideInInspector] public float DamageMultiplier;
    //[HideInInspector] public float CritChance;
    //[HideInInspector] public float CritMultiplier;
    //[HideInInspector] public float AttackSpeedMultiplier;
    //[HideInInspector] public float AreaMultiplier;
    //[HideInInspector] public float ProjectileSpeed;
    //[HideInInspector] public float HealthRegenration;
    //[HideInInspector] public float DamageReductionPercentage;
    //[HideInInspector] public float CollectionRadius;
    //[HideInInspector] public float GoldMultiplier;
    //[HideInInspector] public float XPNeeded;
    //[HideInInspector] public float XPNeededMultiplier;
    //[HideInInspector] public float XPMultiplier;

    public void InitStats()
    {
        //PlayerClassName = PlayerData[m_characterIndex].PlayerClassName;
        //Level = PlayerData[m_characterIndex].Level;
        //MaxHealth = PlayerData[m_characterIndex].MaxHealth;
        //CurrentHealth = MaxHealth;
        //MovementSpeed = PlayerData[m_characterIndex].MovementSpeed;
        //DashAmount = PlayerData[m_characterIndex].DashAmount;
        //DashSpeed = PlayerData[m_characterIndex].DashSpeed;
        //DashCooldown = PlayerData[m_characterIndex].DashCooldown;
        //DashTime = PlayerData[m_characterIndex].DashTime;
        //DamageMultiplier = PlayerData[m_characterIndex].DamageMultiplier;
        //CritChance = PlayerData[m_characterIndex].CritChance;
        //CritMultiplier = PlayerData[m_characterIndex].CritMultiplier;
        //AttackSpeedMultiplier = PlayerData[m_characterIndex].AttackSpeed;
        //AreaMultiplier = PlayerData[m_characterIndex].AreaMultiplier;
        //ProjectileSpeed = PlayerData[m_characterIndex].ProjectileSpeed;
        //HealthRegenration = PlayerData[m_characterIndex].HealthRegeneration;
        //DamageReductionPercentage = PlayerData[m_characterIndex].DamageReductionPercentage;
        //CollectionRadius = PlayerData[m_characterIndex].CollectionRadius;
        //GoldMultiplier = PlayerData[m_characterIndex].GoldMultiplier;
        //XPNeeded = PlayerData[m_characterIndex].XPNeeded;
        //XPNeededMultiplier = PlayerData[m_characterIndex].XPNeededMultiplier;
        //XPMultiplier = PlayerData[m_characterIndex].XPMultiplier;

        ActivePlayerData = Instantiate(PlayerData[m_characterIndex]);
        m_healthBar.fillAmount = this.ActivePlayerData.CurrentHealth / ActivePlayerData.MaxHealth;
    }

    #endregion


    private bool m_isDead;

    public enum AnimationState
    {
        player_run,
        player_idle,
        player_death,
    }

    private AnimationState m_currentState;

    /// <summary>
    /// the Int is the index for the List on the PlayerController to choose which Data and Visual is to be used
    /// </summary>
    /// <param name="_newIndex"></param>

    private void OnEnable()
    {
     
    }

    public void SetCharacterVisualsAndData(int _newIndex)
    {
        m_characterIndex = _newIndex;

        if (m_characterIndex > PlayerData.Count)
        {
            Debug.Log("SetActivePlayerData Error: " + m_characterIndex + "is out of Bounds check if the new index is correct:: No Changes have been Made to ActivePlayerData");
            return;
        }

        ActivePlayerData = PlayerData[m_characterIndex];
        InitStats();

        ProgressionManager.Instance.InitMetaProgression(this);
        ProgressionManager.Instance.UpdateMetaProgression();
       m_spriteRenderer = null;
        m_animator = null;

        for (int i = 0; i < PlayerVisual.Count; ++i)
        {
            if (i == m_characterIndex)
            {
                PlayerVisual[i].SetActive(true);
                m_spriteRenderer = PlayerVisual[i].GetComponent<SpriteRenderer>();
                m_animator = PlayerVisual[i].GetComponent<Animator>();
            }
            else
            {
                PlayerVisual[i].SetActive(false);
            }
        }
    }

    private void Start()
    {
        m_isAbleToDash = true;
        m_isDead = false;
        m_rigidbody = GetComponent<Rigidbody2D>();

        m_characterIndex = 0;
        SetCharacterVisualsAndData(m_characterIndex);

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Village"))
        {
            return;
        }
        else
        {
            GetComponentInChildren<LevelPlayer>().InitXP();
            FindObjectOfType<SpellManager>().InitPassiveData(this);
        }
    }

    private void Update()
    {
        if (ActivePlayerData.CurrentHealth <= 0)
        {
            SetDeathState();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_characterIndex++;
            if (m_characterIndex > PlayerData.Count - 1)
            {
                m_characterIndex = 0;
            }
            if (m_characterIndex == 0) Debug.Log("Archer");
            else if (m_characterIndex == 1) Debug.Log("Swordsman");
            else if (m_characterIndex == 2) Debug.Log("Mage");
            SetCharacterVisualsAndData(m_characterIndex);
            if (MoveDirection.x == 0 && MoveDirection.y == 0) ChangeAnimationState(AnimationState.player_idle);
            else ChangeAnimationState(AnimationState.player_run);
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
        if (MoveDirection.x == 0 && MoveDirection.y == 0) ChangeAnimationState(AnimationState.player_idle);
        else ChangeAnimationState(AnimationState.player_run);
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

        if (m_characterIndex == 0)
        {
            if (m_currentState == newState) return;
            switch (newState)
            {
                case AnimationState.player_idle:
                    animationName = "archer_idle";
                    break;
                case AnimationState.player_run:
                    animationName = "archer_run";
                    break;
                case AnimationState.player_death:
                    animationName = "archer_death";
                    break;
            }
            m_animator.Play(animationName);
            m_currentState = newState;
        }

        else if (m_characterIndex == 1)
        {
            if (m_currentState == newState) return;
            switch (newState)
            {
                case AnimationState.player_idle:
                    animationName = "swordsman_idle";
                    break;
                case AnimationState.player_run:
                    animationName = "swordsman_run";
                    break;
                case AnimationState.player_death:
                    animationName = "swordsman_death";
                    break;
            }
            m_animator.Play(animationName);
            m_currentState = newState;
        }

        else if (m_characterIndex == 2)
        {
            if (m_currentState == newState) return;
            switch (newState)
            {
                case AnimationState.player_idle:
                    animationName = "mage_idle";
                    break;
                case AnimationState.player_run:
                    animationName = "mage_run";
                    break;
                case AnimationState.player_death:
                    animationName = "mage_death";
                    break;
            }
            m_animator.Play(animationName);
            m_currentState = newState;
        }

        else Debug.LogError("Animator out of bounds check m_characterIndex");
    }

    public void GetDamage(float _damageValue)
    {
        this.ActivePlayerData.CurrentHealth -= _damageValue;
        AudioManager.Instance.PlaySound(m_damageTakenClip);
        StartCoroutine(FlashDamage());

        m_healthBar.fillAmount = this.ActivePlayerData.CurrentHealth / ActivePlayerData.MaxHealth;
    }
    IEnumerator FlashDamage()
    {
        m_spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(.2f);

        m_spriteRenderer.color = Color.white;
    }
}
