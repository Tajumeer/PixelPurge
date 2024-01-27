using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemySpawner : MonoBehaviour
{
    #region SubClasses
    [System.Serializable]
    public class Wave
    {
        [Header("Wave Data")]
        public string WaveName;
        public int WaveQuota;
        public float SpawnInterval;
        [HideInInspector] public int SpawnCount;

        [Header("Enemy Data")]
        public List<EnemyGroup> EnemyGroups;
    }

    [System.Serializable]
    public class EnemyGroup
    {
        [Header("Enemy Data")]
        public string EnemyName;
        public int EnemyCount;
        [HideInInspector] public int SpawnCount;
        public GameObject EnemyPrefab;
    }
    #endregion

    [Header("Spawn Control")]
    [SerializeField] private List<Wave> m_waves;
    [SerializeField] private float m_waveInterval;
    [SerializeField] private int m_maxEnemies;
    [SerializeField] private List<Transform> m_SpawnPoints;

    private int m_scorePerWave = 2000;
    private int m_enemiesAlive;
    private bool m_isMaxEnemiesReached;
    private int m_currentWaveCount;
    private Transform m_player;

    private void Start()

    {
        m_player = FindObjectOfType<PlayerController>().transform;
        m_isMaxEnemiesReached = false;

        CalculateWaveQuota();
    }

    private void Update()
    {
        if (m_currentWaveCount < m_waves.Count && m_waves[m_currentWaveCount].SpawnCount == 0)
        {
            StartCoroutine(BeginNextWave());
        }

        if (TimeManager.Instance.GetElapsedTime("WaveSpawnTimer") > m_waves[m_currentWaveCount].SpawnInterval)
        {
            TimeManager.Instance.SetTimer("WaveSpawnTimer", 0f);
            SpawnEnemies();
        }
    }

    private IEnumerator BeginNextWave()
    {
        yield return new WaitForSeconds(m_waveInterval);

        if (m_currentWaveCount < m_waves.Count - 1)
        {
            m_currentWaveCount++;
            GameManager.Instance.AddScore(m_scorePerWave * m_currentWaveCount);
            CalculateWaveQuota();
        }
    }

    private void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;

        foreach (var enemyGroup in m_waves[m_currentWaveCount].EnemyGroups)
        {
            currentWaveQuota += enemyGroup.EnemyCount;
        }

        m_waves[m_currentWaveCount].WaveQuota = currentWaveQuota;
        Debug.Log(currentWaveQuota);
    }

    private void SpawnEnemies()
    {

        if (m_waves[m_currentWaveCount].SpawnCount < m_waves[m_currentWaveCount].WaveQuota && !m_isMaxEnemiesReached)
        {
            foreach (var enemyGroup in m_waves[m_currentWaveCount].EnemyGroups)
            {
                if (enemyGroup.SpawnCount < enemyGroup.EnemyCount)
                {
                    if (m_enemiesAlive >= m_maxEnemies)
                    {
                        m_isMaxEnemiesReached = true;
                        return;
                    }

                    GameObject enemy = Instantiate(enemyGroup.EnemyPrefab, m_player.position + m_SpawnPoints[Random.Range(0, m_SpawnPoints.Count)].position, Quaternion.identity, this.transform);
                    CreateAgent(enemy);

                    enemyGroup.SpawnCount++;
                    m_waves[m_currentWaveCount].SpawnCount++;
                    m_enemiesAlive++;
                }
            }
        }

        if(m_enemiesAlive < m_maxEnemies)
        {
            m_isMaxEnemiesReached = false;
        }
    }

    public void OnEnemyKilled()
    {
        m_enemiesAlive--;
    }

    private void CreateAgent(GameObject _target)
    {

        //if (!_target.GetComponent<NavMeshAgent>())
        //{
        //    _target.AddComponent<NavMeshAgent>();
        //}

        _target.GetComponent<NavMeshAgent>().enabled = false;
        _target.GetComponent<NavMeshAgent>().updateRotation = false;
        _target.GetComponent<NavMeshAgent>().updateUpAxis = false;

        //_target.GetComponent<NavMeshAgent>().baseOffset = .45f;
        //_target.GetComponent<NavMeshAgent>().radius = .4f;
        //_target.GetComponent<NavMeshAgent>().height = .7f;
    }
}
