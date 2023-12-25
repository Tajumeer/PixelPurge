using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    [Header("")]
    [SerializeField] private GameObject m_sandCrawlerPrefab;
    [SerializeField] private int m_waveCount;
    [SerializeField] private float m_waveHPScale;


    // private List<Transform> m_spawns = new List<Transform>();
    private int m_lastSpawn;
    private int m_randomIndex;

    #region SpawnPoints
    public GameObject[] SpawnPointsZero;
    public GameObject[] SpawnPointsOne;
    public GameObject[] SpawnPointsTwo;
    public GameObject[] SpawnPointsThree;
    public GameObject[] SpawnPointsFour;
    public GameObject[] SpawnPointsFive;
    public GameObject[] SpawnPointsSix;
    public GameObject[] SpawnPointsSeven;
    public GameObject[] SpawnPointsEight;
    public GameObject[] SpawnPointsNine;

    private List<GameObject> m_actors = new List<GameObject>();
    private string m_searchTag;
    #endregion

    #region GetSpawnFunctions
    public void GetAllSpawns()
    {
        GetSpawnsInDungeonLevelZero();
        GetSpawnsInDungeonLevelOne();
        GetSpawnsInDungeonLevelTwo();
        GetSpawnsInDungeonLevelThree();
        GetSpawnsInDungeonLevelFour();
        GetSpawnsInDungeonLevelFive();
        GetSpawnsInDungeonLevelSix();
        GetSpawnsInDungeonLevelSeven();
        GetSpawnsInDungeonLevelEight();
        GetSpawnsInDungeonLevelNine();
    }

    private void GetSpawnsInDungeonLevelZero()
    {
        m_searchTag = "DungeonLevel_0";
        FindObjectsWithTag(m_searchTag);
        SpawnPointsZero = m_actors.ToArray();
        m_actors.Clear();




    }

    private void GetSpawnsInDungeonLevelOne()
    {
        m_searchTag = "DungeonLevel_1";
        FindObjectsWithTag(m_searchTag);
        SpawnPointsOne = m_actors.ToArray();
        m_actors.Clear();

    }

    private void GetSpawnsInDungeonLevelTwo()
    {
        m_searchTag = "DungeonLevel_2";
        FindObjectsWithTag(m_searchTag);
        SpawnPointsTwo = m_actors.ToArray();
        m_actors.Clear();
    }

    private void GetSpawnsInDungeonLevelThree()
    {
        m_searchTag = "DungeonLevel_3";
        FindObjectsWithTag(m_searchTag);
        SpawnPointsThree = m_actors.ToArray();
        m_actors.Clear();
    }

    private void GetSpawnsInDungeonLevelFour()
    {
        m_searchTag = "DungeonLevel_4";
        FindObjectsWithTag(m_searchTag);
        SpawnPointsFour = m_actors.ToArray();

        m_actors.Clear();
    }

    private void GetSpawnsInDungeonLevelFive()
    {
        m_searchTag = "DungeonLevel_5";
        FindObjectsWithTag(m_searchTag);
        SpawnPointsFive = m_actors.ToArray();

        m_actors.Clear();
    }

    private void GetSpawnsInDungeonLevelSix()
    {
        m_searchTag = "DungeonLevel_6";
        FindObjectsWithTag(m_searchTag);
        SpawnPointsSix = m_actors.ToArray();

        m_actors.Clear();
    }

    private void GetSpawnsInDungeonLevelSeven()
    {
        m_searchTag = "DungeonLevel_7";
        FindObjectsWithTag(m_searchTag);
        SpawnPointsSeven = m_actors.ToArray();

        m_actors.Clear();
    }

    private void GetSpawnsInDungeonLevelEight()
    {
        m_searchTag = "DungeonLevel_8";
        FindObjectsWithTag(m_searchTag);
        SpawnPointsEight = m_actors.ToArray();

        m_actors.Clear();
    }

    private void GetSpawnsInDungeonLevelNine()
    {
        m_searchTag = "DungeonLevel_9";
        FindObjectsWithTag(m_searchTag);
        SpawnPointsNine = m_actors.ToArray();

        m_actors.Clear();
    }

    #endregion

    private void Start()
    {

    }

    private void FindObjectsWithTag(string _tag)
    {
        m_actors.Clear();
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

    public void SpawnAtRandomPoint(GameObject[] _spawns)
    {
        for (int i = 0; i < _spawns.Length - 1; i++)
        {
            m_randomIndex = RandomizeNumber(_spawns);

            while (m_lastSpawn == m_randomIndex)
            {
                m_randomIndex = RandomizeNumber(_spawns);
            }

            Instantiate(m_sandCrawlerPrefab, _spawns[m_randomIndex].transform.position, Quaternion.identity, this.transform);
            m_lastSpawn = m_randomIndex;
        }
    }

    private int RandomizeNumber(GameObject[] _list)
    {
        return Random.Range(0, _list.Length - 1);
    }
}
