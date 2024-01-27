using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FrostGhostController : MonoBehaviour, IDamagable
{
    public float EnemyDamage;
    public float EnemyHealth;
    [SerializeField] private float m_ExpOnDeath;
    [SerializeField] private int m_ScoreOnDeath;

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
        FrostGhostRun,
        FrostGhostDeath,
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

    }

    void Update()
    {
        if (m_isDead)
        {
            return;
        }
        GetTargetTransform();


        m_agent.enabled = true;

        //else
        //{
        //    m_agent.enabled = false;
        //}

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
        m_levelPlayer.SpawnXP(this.transform.position, m_ExpOnDeath);
        Destroy(m_agent);
        m_isDead = true;
        ChangeAnimationState(AnimationState.FrostGhostDeath);
        Destroy(m_rb);
        Destroy(m_collider);
        m_spriteRenderer.sortingOrder = 0;
        GameManager.Instance.AddScore(m_ScoreOnDeath);
        m_enemySpawner.OnEnemyKilled();
        StartCoroutine(DestroyGameObject(5f));
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
            case AnimationState.FrostGhostRun:
                animationName = "FrostGhostRun";
                break;
            case AnimationState.FrostGhostDeath:
                animationName = "FrostGhostDeath";
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
