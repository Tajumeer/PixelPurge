using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class WaveManager : MonoBehaviour
{

    [Header("Wave Data")]
    [SerializeField] private GameObject m_sandCrawlerPrefab;
    [SerializeField] private int m_waveCount;
    [SerializeField] private float m_waveHPScale;
    [SerializeField] private WaveData[] m_waveDataRoomZero;
    [SerializeField] private Transform m_monsterContainer;


    // private List<Transform> m_spawns = new List<Transform>();
    private int m_lastSpawn;
    private int m_randomIndex;
    private int m_enemyAmount;

    #region SpawnPoints
    // [Header("Spawn Point Arrays")]
    [HideInInspector] public GameObject[] SpawnPointsZero;
    [HideInInspector] public GameObject[] SpawnPointsOne;
    [HideInInspector] public GameObject[] SpawnPointsTwo;
    [HideInInspector] public GameObject[] SpawnPointsThree;
    [HideInInspector] public GameObject[] SpawnPointsFour;
    [HideInInspector] public GameObject[] SpawnPointsFive;
    [HideInInspector] public GameObject[] SpawnPointsSix;
    [HideInInspector] public GameObject[] SpawnPointsSeven;
    [HideInInspector] public GameObject[] SpawnPointsEight;
    [HideInInspector] public GameObject[] SpawnPointsNine;

    private List<GameObject> m_actors = new List<GameObject>();
    #endregion

    public void Initialize()
    {
        GetAllSpawns();
        StartCoroutine(RoomZeroControl(5f));
    }
    private IEnumerator RoomZeroControl(float _timeAfterFirstSpawn)
    {
        SpawnAtRandomPointsInRoom(SpawnPointsZero, 0, 0);
        CreateAgents();
        yield return new WaitForSeconds(_timeAfterFirstSpawn); 
        SpawnAtRandomPointsInRoom(SpawnPointsZero, 1, 0);
        CreateAgents();
    }

    private void GetAllSpawns()
    {

        GetSpawns("DungeonLevel_0", ref SpawnPointsZero);
        Debug.Log(SpawnPointsZero.Length.ToString());
        GetSpawns("DungeonLevel_1", ref SpawnPointsOne);
        GetSpawns("DungeonLevel_2", ref SpawnPointsTwo);
        GetSpawns("DungeonLevel_3", ref SpawnPointsThree);
        GetSpawns("DungeonLevel_4", ref SpawnPointsFour);
        GetSpawns("DungeonLevel_5", ref SpawnPointsFive);
        GetSpawns("DungeonLevel_6", ref SpawnPointsSix);
        GetSpawns("DungeonLevel_7", ref SpawnPointsSeven);
        GetSpawns("DungeonLevel_8", ref SpawnPointsEight);
        GetSpawns("DungeonLevel_9", ref SpawnPointsNine);
    }

    private void GetSpawns(string _searchTag, ref GameObject[] _targetArray)
    {
        string searchTag = _searchTag;
        FindObjectsWithTag(searchTag);
        _targetArray = m_actors.ToArray();
        m_actors.Clear();
    }

    private void FindObjectsWithTag(string _tag)
    {
        Transform parent = GameObject.FindGameObjectWithTag(_tag).transform;
        GetSpawnInChild(parent, "Spawn");
    }

    private void GetSpawnInChild(Transform _parent, string _tag)
    {
        for (int i = 0; i < _parent.childCount; i++)
        {
            Transform child = _parent.GetChild(i);
            if (child.tag == _tag)
            {
                m_actors.Add(child.gameObject);
            }
            if (child.childCount > 0)
            {
                GetSpawnInChild(child, _tag);
            }
        }
    }

    private void SpawnAtRandomPointsInRoom(GameObject[] _spawns, int _waveNumber, int _enemyType)
    {


        for (int i = 0; i < _spawns.Length - 1; i++)
        {
            m_randomIndex = RandomizeNumber(_spawns);

            while (m_lastSpawn == m_randomIndex)
            {
                m_randomIndex = RandomizeNumber(_spawns);
            }

            if (m_enemyAmount <= m_waveDataRoomZero[_waveNumber].EnemyCount)
            {
                Instantiate(m_waveDataRoomZero[_waveNumber].EnemyTypes[_enemyType], _spawns[m_randomIndex].transform.position, Quaternion.identity, m_monsterContainer);
                m_lastSpawn = m_randomIndex;
                m_enemyAmount++;
            }
            else { break; }

        }
    }

    private int RandomizeNumber(GameObject[] _list)
    {
        return Random.Range(0, _list.Length - 1);
    }

    private void CreateAgents()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject go in enemies)
        {
            if (!go.GetComponent<NavMeshAgent>())
            {
                go.AddComponent<NavMeshAgent>();
            }
            go.GetComponent<NavMeshAgent>().enabled = false;
            go.GetComponent<NavMeshAgent>().baseOffset = .45f;
            go.GetComponent<NavMeshAgent>().radius = .4f;
            go.GetComponent<NavMeshAgent>().height = .7f;
            go.GetComponent<NavMeshAgent>().updateRotation = false;
            go.GetComponent<NavMeshAgent>().updateUpAxis = false;
        }
    }
}
