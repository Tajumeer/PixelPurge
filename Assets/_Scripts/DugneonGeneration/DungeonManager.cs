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

    [SerializeField] private int m_middleRoomAmount;

    [Header("Dungeon Room Prefabs")]
    [SerializeField] private List<GameObject> m_startRooms;
    [SerializeField] private List<GameObject> m_middleRooms;
    [SerializeField] private List<GameObject> m_endRooms;

    [Header("NavMesh")]
    [SerializeField] private NavMeshSurface m_navMeshSurface;

    private List<GameObject> m_spawnedRooms;
    private int m_roomCount;
    private Vector3 startPos;


    private void Start()
    {
        m_spawnedRooms = new List<GameObject>();
        startPos = Vector3.zero;

        SpawnDungeonRooms();

        //Generate NavMesh in Runtime
        m_navMeshSurface.BuildNavMeshAsync();

        GameObject.FindObjectOfType<WaveManager>().Initialize();

        //Gives each with enemy Tagged enemy a NavMeshAgent
        CreateAgents();


    }

    private void SpawnDungeonRooms()
    {

        Instantiate(m_startRooms[RandomizeNumber(m_startRooms)], new Vector3(startPos.x + (m_roomCount * F_ROOM_WIDTH), startPos.y, startPos.z), Quaternion.identity, this.transform);
        m_roomCount++;

        for (int i = 0; i < m_middleRoomAmount; i++)
        {
            SpawnUniqueRoom(m_middleRooms);
        }

        Instantiate(m_endRooms[RandomizeNumber(m_endRooms)], new Vector3(startPos.x + (m_roomCount * F_ROOM_WIDTH), startPos.y, startPos.z), Quaternion.identity, this.transform);
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
        Instantiate(roomPrefab, new Vector3(startPos.x + (m_roomCount * F_ROOM_WIDTH), startPos.y, startPos.z), Quaternion.identity, this.transform);
        m_roomCount++;
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
