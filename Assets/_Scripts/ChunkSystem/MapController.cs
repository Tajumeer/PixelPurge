using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Purchasing;
using UnityEngine;

public class MapController : MonoBehaviour
{
    //private const float F_CHUNK_SIZE = 19.8f;
    [Header("Chunk Creation")]
    [SerializeField] private GameObject m_chunkContainer;
    [SerializeField] private GameObject m_player;
    [SerializeField] private float m_checkRadius;
    [SerializeField] private LayerMask m_terrainMask;
    [SerializeField] private List<GameObject> m_terrainChunks;
    [HideInInspector] public GameObject CurrentChunk;

    private Vector3 m_lastPlayerPosition;

    [Header("Load Reduction")]
    [SerializeField] private float m_maxOpDist;
    [SerializeField] private float m_cleanUpCooldown;
    public List<GameObject> SpawnedChunks;
    private GameObject m_latestChunk;
    private float m_opDist;


    void Start()
    {
        TimeManager.Instance.StartTimer("ChunkCleanupCooldown");
        m_lastPlayerPosition = m_player.transform.position;
    }


    void Update()
    {
        ChunkChecker();
        ChunkCleaner();
    }

    private void ChunkChecker()
    {
        if (!CurrentChunk)
        {
            return;
        }

        Vector3 moveDir = m_player.transform.position - m_lastPlayerPosition;
        m_lastPlayerPosition = m_player.transform.position;

        string directionName = GetDirectionName(moveDir);

        CheckAndSpawnChunk(directionName);

        if(directionName.Contains("Top"))
        {
            CheckAndSpawnChunk("Top");
        }
        if (directionName.Contains("Bottom"))
        {
            CheckAndSpawnChunk("Bottom");
        }
        if (directionName.Contains("Right"))
        {
            CheckAndSpawnChunk("Right");
        }
        if (directionName.Contains("Left"))
        {
            CheckAndSpawnChunk("Left");
        }
    }

    private void CheckAndSpawnChunk(string _direction)
    {
        if(!Physics2D.OverlapCircle(CurrentChunk.transform.Find(_direction).position, m_checkRadius, m_terrainMask))
        {
            SpawnChunk(CurrentChunk.transform.Find(_direction).position);
        }
    }

    private string GetDirectionName(Vector3 _direction)
    {
        _direction = _direction.normalized;

        if (Mathf.Abs(_direction.x) > Mathf.Abs(_direction.y))
        {
            if (_direction.y > 0.5f)
            {
                return _direction.x > 0 ? "TopRight" : "TopLeft";
            }
            else if (_direction.y < -0.5f)
            {
                return _direction.x > 0 ? "BottomRight" : "BottomLeft";
            }
            else
            {
                return _direction.x > 0 ? "Right" : "Left";
            }
        }
        else
        {
            if (_direction.x > 0.5f)
            {
                return _direction.y > 0 ? "TopRight" : "BottomRight";
            }
            else if (_direction.x < -0.5f)
            {
                return _direction.y > 0 ? "TopLeft" : "BottomLeft";
            }
            else
            {
                return _direction.y > 0 ? "Top" : "Bottom";
            }
        }
    }

    private void SpawnChunk(Vector3 _spawnPosition)
    {
        int rnd = Random.Range(0, m_terrainChunks.Count);
        m_latestChunk = Instantiate(m_terrainChunks[rnd], _spawnPosition, Quaternion.identity, m_chunkContainer.transform);
        SpawnedChunks.Add(m_latestChunk);
    }

    private void ChunkCleaner()
    {
        if (TimeManager.Instance.GetElapsedTime("ChunkCleanupCooldown") >= m_cleanUpCooldown)
        {
            TimeManager.Instance.SetTimer("ChunkCleanupCooldown", 0f);
        }
        else
        {
            return;
        }

        foreach (GameObject chunk in SpawnedChunks)
        {
            m_opDist = Vector3.Distance(m_player.transform.position, chunk.transform.position);
            if (m_opDist > m_maxOpDist)
            {
                chunk.SetActive(false);
            }
            else
            {
                chunk.SetActive(true);
            }
        }
    }
}
