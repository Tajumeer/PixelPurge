using JetBrains.Annotations;
using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DungeonManager : MonoBehaviour
{
    private const int I_MID_ROOM_SIZE = 8;
    private const float F_ROOM_WIDTH = 35.6f;
    private const string S_DUNGEON_TAG = "DungeonLevel_";


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

        m_waveManager.Initialize();
    }

    private void SpawnDungeonRooms()
    {

        m_currentRoom = Instantiate(m_startRooms[RandomizeNumber(m_startRooms)], new Vector3(startPos.x + (m_roomCount * F_ROOM_WIDTH), startPos.y, startPos.z), Quaternion.identity, this.transform);
        m_currentRoom.tag = S_DUNGEON_TAG + m_roomCount.ToString();
        m_roomCount++;

        for (int i = 0; i < I_MID_ROOM_SIZE; i++)
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

   

}
