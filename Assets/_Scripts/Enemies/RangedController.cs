using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class RangedController : MonoBehaviour, IDamagable
{
    [SerializeField] private GameObject m_projectilePrefab;
    [SerializeField] private GameObject m_projectileParent;

    private NavMeshAgent m_agent;
    private Transform m_target;
    private Animator m_animator;
    private Collider2D m_collider;
    private Rigidbody2D m_rb;
    private SpriteRenderer m_spriteRenderer;
    private EnemySpawner m_enemySpawner;
    private LevelPlayer m_levelPlayer;


    public float EnemyDamage;
    public float EnemyHealth;
    [SerializeField] private float m_ExpOnDeath;
    [SerializeField] private int m_ScoreOnDeath;
    [SerializeField] private int m_goldOnDeath;
    [SerializeField] private float m_attackFrequency;
    [SerializeField] private float m_projectileSpeed;
    [SerializeField] private float m_attackRange;
    [SerializeField] private LayerMask m_playerLayer;
    [SerializeField] private List<ItemDrop> m_DropList;
    private float m_attackFrequencyTimer;


    private bool m_isDead;

    private enum AnimationState
    {
        FrostCrawlerRun,
        FrostCrawlerDeath,
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

        m_attackFrequencyTimer = 0f;
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

        if (TargetIsInRange(m_target.position))
        {
            m_agent.SetDestination(this.transform.position);

            m_attackFrequencyTimer += Time.deltaTime;

            if (m_attackFrequencyTimer >= m_attackFrequency)
            {
                Shoot();
                m_attackFrequencyTimer = 0f;
            }

            return;
        }
        else if (m_agent.enabled && m_agent.isOnNavMesh)
        {
            m_agent.SetDestination(m_target.position);
        }
    }

    private void Shoot()
    {
        Vector3 direction = (m_target.transform.position - transform.position).normalized;

        GameObject projectile = Instantiate(m_projectilePrefab, this.transform.position, Quaternion.identity, m_projectileParent.transform);

        projectile.GetComponent<Rigidbody2D>().velocity = direction * m_projectileSpeed;

        projectile.transform.Rotate(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
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

        m_levelPlayer.SpawnXP(this.transform.position, m_ExpOnDeath);
        Destroy(m_agent);
        m_isDead = true;
        ChangeAnimationState(AnimationState.FrostCrawlerDeath);
        Destroy(m_rb);
        Destroy(m_collider);
        m_spriteRenderer.sortingOrder = 0;
        m_enemySpawner.OnEnemyKilled();
        GameManager.Instance.AddScore(m_ScoreOnDeath);
        GameManager.Instance.AddGold(m_goldOnDeath);
        GetComponent<DeathBool>().IsDead = true;
        StartCoroutine(DestroyGameObject(2f));
    }

    private IEnumerator DestroyGameObject(float _corpseTime)
    {
        yield return new WaitForSeconds(_corpseTime);
        Destroy(this.gameObject);
    }

    private bool TargetIsInRange(Vector2 _targetPosition)
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, m_attackRange, Vector2.zero, 0f, m_playerLayer);

        return hit.collider != null && hit.collider.CompareTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.LogWarning("Hit Player!");
            collision.GetComponent<IDamagable>().GetDamage(EnemyDamage);
        }
    }

    private void ChangeAnimationState(AnimationState newState)
    {
        string animationName = string.Empty;
        if (m_currentState == newState) return;
        switch (newState)
        {
            case AnimationState.FrostCrawlerRun:
                animationName = "FrostCrawlerRun";
                break;
            case AnimationState.FrostCrawlerDeath:
                animationName = "FrostCrawlerDeath";
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


