using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "WaveData", menuName = "PixelPurge/WaveData", order = 1)]
public class WaveData : ScriptableObject
{
    public int[] EnemyTypeCount;
    public GameObject[] EnemyTypes;
    public float SpawnTime;
    public bool IsSpawned = false;
}
