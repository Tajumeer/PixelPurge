using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class WaveManager : MonoBehaviour
{

    [Header("Variables")]
    [SerializeField] private float m_waveHPScale;
    [SerializeField] private Transform m_monsterContainer;

    [Header("WaveData")]
    [SerializeField] private WaveData[] m_waveDataRoomZero;
    [SerializeField] private WaveData[] m_waveDataRoomOne;
    [SerializeField] private WaveData[] m_waveDataRoomTwo;
    [SerializeField] private WaveData[] m_waveDataRoomThree;
    [SerializeField] private WaveData[] m_waveDataRoomFour;
    [SerializeField] private WaveData[] m_waveDataRoomFive;
    [SerializeField] private WaveData[] m_waveDataRoomSix;
    [SerializeField] private WaveData[] m_waveDataRoomSeven;
    [SerializeField] private WaveData[] m_waveDataRoomEight;
    [SerializeField] private WaveData[] m_waveDataRoomNine;

    //private WaveData[] m_waveData;

    // private List<Transform> m_spawns = new List<Transform>();
    private int m_lastSpawn;
    private int m_randomIndex;
    private int m_enemyAmount;

    private int m_currentRoom;
    private int m_activeEnemies;

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

    private TimeManager m_timeManager;
    private List<GameObject> m_actors = new List<GameObject>();
    private List<GameObject> m_exits = new List<GameObject>();

    #endregion
    private void Awake()
    {

        m_timeManager = TimeManager.Instance;

        foreach (var b in m_waveDataRoomZero)
        {
            b.IsSpawned = false;
        }
    }
    public void Initialize()
    {
        m_currentRoom = 0;
        GetAllSpawns();
        m_timeManager.StartTimer("WaveTimer");

        //SpawnWave(SpawnPointsSeven, 0, 0, m_waveDataRoomSeven);
        //SpawnWave(SpawnPointsFour, 0, 0, m_waveDataRoomFour);


    }

    private void Update()
    {
        m_activeEnemies = GameObject.FindObjectsOfType<NavMeshAgent>().Length;

    }

    private void FixedUpdate()
    {

        RoomControl();


    }

    private void RoomControl()
    {
        switch (m_currentRoom)
        {
            case 0:
                DungeonRoomZero();
                break;

        }
    }
    private void DungeonRoomZero()
    {
        bool allTrue = m_waveDataRoomZero.All(b => b.IsSpawned);
        

        //Controls Wave 0
        if (m_timeManager.GetElapsedTime("WaveTimer") > m_waveDataRoomZero[0].SpawnTime && !m_waveDataRoomZero[0].IsSpawned)
        {
            SpawnWave(SpawnPointsZero, 0, 0, m_waveDataRoomZero);
            m_waveDataRoomZero[0].IsSpawned = true;
        }

        //Controls Wave 1
        if (m_timeManager.GetElapsedTime("WaveTimer") > m_waveDataRoomZero[1].SpawnTime && !m_waveDataRoomZero[1].IsSpawned)
        {
            SpawnWave(SpawnPointsZero, 1, 0, m_waveDataRoomZero);
            SpawnWave(SpawnPointsZero, 1, 1, m_waveDataRoomZero);
            m_waveDataRoomZero[1].IsSpawned = true;
        }

        //Controls Wave 2
        if (m_timeManager.GetElapsedTime("WaveTimer") > m_waveDataRoomZero[2].SpawnTime && !m_waveDataRoomZero[2].IsSpawned)
        {
            SpawnWave(SpawnPointsZero, 2, 0, m_waveDataRoomZero);
            SpawnWave(SpawnPointsZero, 2, 1, m_waveDataRoomZero);
            m_waveDataRoomZero[2].IsSpawned = true;
        }


        //Opens Exit
        else if (m_activeEnemies == 0 && allTrue)
        {
            FindObjectsWithTag("DungeonLevel_0", "Exit", m_exits);

            foreach (var go in m_exits)
            {
                go.SetActive(false);
            }

            m_currentRoom++;
            Debug.Log(m_timeManager.GetElapsedTime("WaveTimer"));
            m_timeManager.SetTimer("WaveTimer", 0f);
            Debug.Log(m_timeManager.GetElapsedTime("WaveTimer"));
            Debug.Log("Room Zero Cleared!");

        }
    }


    #region Spawns
    private void GetAllSpawns()
    {

        GetSpawns("DungeonLevel_0", ref SpawnPointsZero);
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
        FindObjectsWithTag(searchTag, "Spawn", m_actors);
        _targetArray = m_actors.ToArray();
        m_actors.Clear();
    }

    private void FindObjectsWithTag(string _parentTag, string _childTag, List<GameObject> _storage)
    {
        Transform parent = GameObject.FindGameObjectWithTag(_parentTag).transform;
        GetTagInChild(parent, _childTag, _storage);
    }

    private void GetTagInChild(Transform _parent, string _tag, List<GameObject> _storage)
    {
        for (int i = 0; i < _parent.childCount; i++)
        {
            Transform child = _parent.GetChild(i);
            if (child.tag == _tag)
            {
                _storage.Add(child.gameObject);
            }
            if (child.childCount > 0)
            {
                GetTagInChild(child, _tag, m_actors);
            }
        }
    }

    #endregion
    private void SpawnWave(GameObject[] _spawns, int _waveNumber, int _enemyType, WaveData[] _roomData)
    {
        //GameObject spawnContainer = new GameObject("Room: " + m_currentRoom);
        //if (spawnContainer != null)
        //    {
        //    Instantiate(spawnContainer, m_monsterContainer);
        //}

        for (int i = 0; i < _spawns.Length - 1; i++)
        {
            m_randomIndex = RandomizeNumber(_spawns);

            while (m_lastSpawn == m_randomIndex)
            {
                m_randomIndex = RandomizeNumber(_spawns);
            }


            if (m_enemyAmount <= _roomData[_waveNumber].EnemyTypeCount[_enemyType] - 1)
            {
                m_enemyAmount++;
                GameObject enemy = Instantiate(_roomData[_waveNumber].EnemyTypes[_enemyType], _spawns[m_randomIndex].transform.position, Quaternion.identity, m_monsterContainer);
                CreateAgent(enemy);
                m_lastSpawn = m_randomIndex;
            }
            else { break; }
        }
        m_enemyAmount = 0;
    }

    private int RandomizeNumber(GameObject[] _list)
    {
        return Random.Range(0, _list.Length - 1);
    }

    private void CreateAgent(GameObject _target)
    {

        if (!_target.GetComponent<NavMeshAgent>())
        {
            _target.AddComponent<NavMeshAgent>();
        }
        _target.GetComponent<NavMeshAgent>().enabled = false;
        //_target.GetComponent<NavMeshAgent>().baseOffset = .45f;
        //_target.GetComponent<NavMeshAgent>().radius = .4f;
        //_target.GetComponent<NavMeshAgent>().height = .7f;
        _target.GetComponent<NavMeshAgent>().updateRotation = false;
        _target.GetComponent<NavMeshAgent>().updateUpAxis = false;

    }
}
