using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "PixelPurge/WaveData", order = 1)]
public class WaveData : ScriptableObject
{
    public int EnemyCount;
    public GameObject[] EnemyTypes;

}
