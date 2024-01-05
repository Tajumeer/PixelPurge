using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRandomizer : MonoBehaviour
{
    public List<GameObject> PropSpawns;
    public List<GameObject> PropPrefabs;

    private void Start()
    {
        SpawnProps();
    }

    private void SpawnProps()
    {
        foreach (GameObject spawn in PropSpawns)
        {
            int rnd = Random.Range(0, PropPrefabs.Count);

            Instantiate(PropPrefabs[rnd], spawn.transform.position, Quaternion.identity, spawn.transform);
        }
    }
}
