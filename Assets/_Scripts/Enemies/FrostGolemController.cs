using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FrostGolemController : MonoBehaviour, IDamagable
{
    public float EnemyDamage;
    public float EnemyHealth;
    [SerializeField] private float m_ExpOnDeath;
    [SerializeField] private int m_ScoreOnDeath;
    [SerializeField] private int m_goldOnDeath;
    [SerializeField] private List<ItemDrop> m_DropList;

    private NavMeshAgent m_agent;
    private Transform m_target;
    private Animator m_animator;
    private Collider2D m_collider;
    private Rigidbody2D m_rb;
    private SpriteRenderer m_spriteRenderer;

    private EnemySpawner m_enemySpawner;
    private LevelPlayer m_levelPlayer;

    private bool m_isDead;
    private enum AnimationState
    {
        FrostGolemRun,
        FrostGolemDeath,
    }
    private AnimationState m_currentState;

    void Start()
    {
        m_animator = GetComponentInChildren<Animator>();
        m_agent = GetComponent<NavMeshAgent>();
        m_collider = GetComponent<Collider2D>();
        m_rb = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        m_levelPlayer = FindObjectOfType<LevelPlayer>();
        m_enemySpawner = FindObjectOfType<EnemySpawner>();

        EnemyHealth = GameManager.Instance.GetScaledHP(EnemyHealth);
    }

    void Update()
    {
        if (m_isDead)
        {
            return;
        }
        GetTargetTransform();


        m_agent.enabled = true;

        if (m_agent.enabled && m_agent.isOnNavMesh)
        {
            m_agent.SetDestination(m_target.position);
        }
    }

    private void LateUpdate()
    {
        if (EnemyHealth > 0f)
        {
            return;
        }

        SetDeathState();
    }

    private void GetTargetTransform()
    {
        m_target = GameObject.FindWithTag("Player").transform;
    }

    public void SetDeathState()
    {
        if (m_isDead) { return; }

        if (m_DropList != null)
        {
            foreach (ItemDrop drop in m_DropList)
            {
                drop.DropItem();
            }
        }

        Destroy(m_agent);
        m_isDead = true;
        ChangeAnimationState(AnimationState.FrostGolemDeath);
        Destroy(m_rb);
        Destroy(m_collider);
        m_enemySpawner.OnEnemyKilled();
        m_spriteRenderer.sortingOrder = 0;
        GameManager.Instance.AddScore(m_ScoreOnDeath);
        GameManager.Instance.AddGold(m_goldOnDeath);
        m_levelPlayer.SpawnXP(this.transform.position, m_ExpOnDeath);
        GetComponent<DeathBool>().IsDead = true;
        StartCoroutine(DestroyGameObject(2f));
    }

    private IEnumerator DestroyGameObject(float _corpseTime)
    {
        yield return new WaitForSeconds(_corpseTime);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<IDamagable>().GetDamage(EnemyDamage);
        }
    }


    private void ChangeAnimationState(AnimationState newState)
    {
        string animationName = string.Empty;
        if (m_currentState == newState) return;
        switch (newState)
        {
            case AnimationState.FrostGolemRun:
                animationName = "FrostGolemRun";
                break;
            case AnimationState.FrostGolemDeath:
                animationName = "FrostGolemDeath";
                break;
        }
        m_animator.Play(animationName);
        m_currentState = newState;
    }

    public void GetDamage(float _damageValue)
    {
        this.EnemyHealth -= _damageValue;
        StartCoroutine(FlashDamage());
    }

    IEnumerator FlashDamage()
    {
        m_spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(.2f);

        m_spriteRenderer.color = Color.white;
    }
}

