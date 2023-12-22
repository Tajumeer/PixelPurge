using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    private const int M_ROOM_WIDTH = 36;

    [SerializeField] private int m_middleRoomAmount;

    [Header("Dungeon Room Prefabs")]
    [SerializeField] private List<GameObject> m_startRooms;
    [SerializeField] private List<GameObject> m_middleRooms;
    [SerializeField] private List<GameObject> m_endRooms;
   private int m_roomCount;


    private void Start()
    {
        SpawnDungeonRooms();
    }

    private void SpawnDungeonRooms()
    {
        Vector3 startPos = Vector3.zero;

        Instantiate(m_startRooms[RandomizeNumber(m_startRooms)], new Vector3(startPos.x + (m_roomCount * M_ROOM_WIDTH), startPos.y, startPos.z), Quaternion.identity, this.transform);
        m_roomCount++;

        for (int i = 0; i < m_middleRoomAmount; i++)
        {
            Instantiate(m_middleRooms[RandomizeNumber(m_middleRooms)], new Vector3(startPos.x + (m_roomCount * M_ROOM_WIDTH), startPos.y, startPos.z), Quaternion.identity, this.transform);
            m_roomCount++; 
        }

        Instantiate(m_endRooms[RandomizeNumber(m_endRooms)], new Vector3(startPos.x + (m_roomCount * M_ROOM_WIDTH), startPos.y, startPos.z), Quaternion.identity, this.transform);
    }
    private int RandomizeNumber(List<GameObject> _list)
    {
        return Random.Range(0, _list.Count);
    }
 
}
