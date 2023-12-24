using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    [Header("")]
    [SerializeField] private GameObject m_sandCrawlerPrefab;
    [SerializeField] private int m_waveCount;
    [SerializeField] private float m_waveHPScale;

    //private Transform m_enemySpawn;
    private GameObject[] m_spawnPoints;
    private List<Transform> m_spawns;
    private int m_lastSpawn;
    private int m_randomIndex;
  
    public void Initialize()
    {
        m_spawns = new List<Transform>();
        m_spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");

        foreach (GameObject go in m_spawnPoints)
        {
            m_spawns.Add(go.transform);
        }

        SpawnAtRandomPoint();
    }


    void SpawnAtRandomPoint()
    {
        for (int i = 0; i < m_spawns.Count; i++)
        {
            m_randomIndex = RandomizeNumber(m_spawns);

            while (m_lastSpawn == m_randomIndex)
            {
                m_randomIndex = RandomizeNumber(m_spawns);
            }

            Instantiate(m_sandCrawlerPrefab, m_spawns[m_randomIndex].position, Quaternion.identity, this.transform);
            m_lastSpawn = m_randomIndex;
        }
    }

    private int RandomizeNumber(List<Transform> _list)
    {
        return Random.Range(0, _list.Count);
    }
}
