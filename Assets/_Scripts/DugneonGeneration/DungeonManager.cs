using JetBrains.Annotations;
using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DungeonManager : MonoBehaviour
{
    private const float F_ROOM_WIDTH = 35.6f;
    private const string S_DUNGEON_TAG = "DungeonLevel_";

    [SerializeField] private int m_middleRoomAmount;

    [Header("Dungeon Room Prefabs")]
    [SerializeField] private List<GameObject> m_startRooms;
    [SerializeField] private List<GameObject> m_middleRooms;
    [SerializeField] private List<GameObject> m_endRooms;

    [Header("NavMesh")]
    [SerializeField] private NavMeshSurface m_navMeshSurface;

    private List<GameObject> m_spawnedRooms;
    private GameObject m_currentRoom;
    private int m_roomCount;
    private Vector3 startPos;
    private WaveManager m_waveManager;


    private void Start()
    {
        m_waveManager = GameObject.FindObjectOfType<WaveManager>();

        m_spawnedRooms = new List<GameObject>();
        startPos = Vector3.zero;

        SpawnDungeonRooms();

        //Generate NavMesh in Runtime
        m_navMeshSurface.BuildNavMeshAsync();

        GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.FindFirstObjectByType<PlayerSpawn>().transform.position;
        m_waveManager.GetAllSpawns();
        m_waveManager.SpawnAtRandomPoint(m_waveManager.SpawnPointsZero);



        //Gives each with enemy Tagged enemy a NavMeshAgent
        CreateAgents();


    }

    private void SpawnDungeonRooms()
    {

        m_currentRoom = Instantiate(m_startRooms[RandomizeNumber(m_startRooms)], new Vector3(startPos.x + (m_roomCount * F_ROOM_WIDTH), startPos.y, startPos.z), Quaternion.identity, this.transform);
        m_currentRoom.tag = S_DUNGEON_TAG + m_roomCount.ToString();
        m_roomCount++;

        for (int i = 0; i < m_middleRoomAmount; i++)
        {
            SpawnUniqueRoom(m_middleRooms);
            m_currentRoom.tag = S_DUNGEON_TAG + m_roomCount.ToString();
            m_roomCount++;
        }

        m_currentRoom = Instantiate(m_endRooms[RandomizeNumber(m_endRooms)], new Vector3(startPos.x + (m_roomCount * F_ROOM_WIDTH), startPos.y, startPos.z), Quaternion.identity, this.transform);
        m_currentRoom.tag = S_DUNGEON_TAG + m_roomCount.ToString();
    }

    private void SpawnUniqueRoom(List<GameObject> _roomList)
    {
        int randomIndex = RandomizeNumber(_roomList);
        GameObject roomPrefab = _roomList[randomIndex];

        if (m_spawnedRooms != null)
        {
            while (m_spawnedRooms.Contains(roomPrefab))
            {
                randomIndex = RandomizeNumber(_roomList);
                roomPrefab = _roomList[randomIndex];
            }

        }

        m_spawnedRooms.Add(roomPrefab);
        m_currentRoom = Instantiate(roomPrefab, new Vector3(startPos.x + (m_roomCount * F_ROOM_WIDTH), startPos.y, startPos.z), Quaternion.identity, this.transform);

    }

    private int RandomizeNumber(List<GameObject> _list)
    {
        return Random.Range(0, _list.Count);
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
