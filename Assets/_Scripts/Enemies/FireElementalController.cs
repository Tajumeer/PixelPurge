using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FireElementalController : MonoBehaviour, IDamagable
{
    public float EnemyDamage;
    public float EnemyHealth;
    [SerializeField] private float m_ExpOnDeath;

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
        fireEle_run,
        fireEle_death,
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
        // m_agent.SetDestination(this.m_target.position);
        Destroy(m_agent);
        m_isDead = true;
        ChangeAnimationState(AnimationState.fireEle_death);
        Destroy(m_rb);
        Destroy(m_collider);
   
        m_enemySpawner.OnEnemyKilled();
        m_spriteRenderer.sortingOrder = 0;
     
        m_levelPlayer.SpawnXP(this.transform.position, m_ExpOnDeath);
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
            case AnimationState.fireEle_run:
                animationName = "fireEle_run";
                break;
            case AnimationState.fireEle_death:
                animationName = "fireEle_death";
                break;
        }
        m_animator.Play(animationName);
        m_currentState = newState;
    }

    public void GetDamage(float _damageValue)
    {
        this.EnemyHealth -= _damageValue;
    }
}
