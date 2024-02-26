using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public enum Characters
{
    Archer = 0,
    Swordsman = 1,
    Mage = 2
}

public class PlayerController : MonoBehaviour, IDamagable
{
    [SerializeField] private SO_AllSpells m_allSpellsSO;
    public List<PlayerStats> PlayerData;
    public List<GameObject> PlayerVisual;
    [SerializeField] private AudioClip m_damageTakenClip;
    [HideInInspector] public PlayerStats ActivePlayerData;
    [HideInInspector] public int m_classIndex;
    [SerializeField] private float InternalHealthRegCD;

    private int m_characterIndex;

    private bool m_isAbleToDash;
    private bool m_isDashing;

    [SerializeField] private Image m_healthBar;

    [Header("Components")]
    [HideInInspector] public Vector2 MoveDirection;
    private Rigidbody2D m_rigidbody;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;

    public void InitStats()
    {
        ActivePlayerData = Instantiate(PlayerData[m_characterIndex]);
        m_allSpellsSO.statSO = ActivePlayerData;
        m_healthBar.fillAmount = this.ActivePlayerData.CurrentHealth / ActivePlayerData.MaxHealth;
    }

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

    public void UpdateHealthbar()
    {
        m_healthBar.fillAmount = this.ActivePlayerData.CurrentHealth / ActivePlayerData.MaxHealth;
    }

    public void SetCharacterVisualsAndData(Characters _newCharacter)
    {
        m_characterIndex = (int)_newCharacter;

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

        m_characterIndex = GameManager.Instance.ClassIndex;
        SetCharacterVisualsAndData((Characters)m_characterIndex);

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Village"))
        {
            return;
        }
        else
        {
            GetComponentInChildren<LevelPlayer>().InitXP();
        }

        TimeManager.Instance.StartTimer("HealthRegInternalTimer");
    }

    private void Update()
    {
        if (ActivePlayerData.CurrentHealth <= 0)
        {
            SetDeathState();
        }

        SetAnimation(); 

        if (m_isDead) { return; }

        if (TimeManager.Instance.GetElapsedTime("HealthRegInternalTimer") > InternalHealthRegCD)
        {
            RegenerateHealth();
        }

        if (m_isDashing) { return; }

        Inputs();

        if (Input.GetKey(KeyCode.Space) && m_isAbleToDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void RegenerateHealth()
    {
        ActivePlayerData.CurrentHealth += ActivePlayerData.HealthRegeneration;

        if(ActivePlayerData.CurrentHealth > ActivePlayerData.MaxHealth)
        {
            ActivePlayerData.CurrentHealth = ActivePlayerData.MaxHealth;
        }

        UpdateHealthbar();
        TimeManager.Instance.SetTimer("HealthRegInternalTimer", 0f);
    }

    private void SetDeathState()
    {
        if (m_isDead) return;

        m_isDead = true;
        m_spriteRenderer.sortingOrder = 0;
        MoveDirection = Vector3.zero;
        GameManager.Instance.Lose();
        TimeManager.Instance.StopTimer("HealthRegInternalTimer");
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
    }

    public void GetDamage(float _damageValue)
    {
        this.ActivePlayerData.CurrentHealth -= CalculateReducedDamage(_damageValue);
        AudioManager.Instance.PlaySound(m_damageTakenClip);
        StartCoroutine(FlashDamage());

        m_healthBar.fillAmount = this.ActivePlayerData.CurrentHealth / ActivePlayerData.MaxHealth;
    }

    private float CalculateReducedDamage(float _rawDamage)
    {
        float reductionAmount = _rawDamage * (ActivePlayerData.DamageReductionPercentage / 100f);
        float reducedDamage = _rawDamage - reductionAmount;

        reducedDamage = Mathf.Max(reducedDamage, 0f);

        return reducedDamage;
    }

    IEnumerator FlashDamage()
    {
        m_spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(.2f);

        m_spriteRenderer.color = Color.white;
    }
}
