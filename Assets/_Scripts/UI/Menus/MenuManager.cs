using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script: Maya

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [Header("Scene Numbers in Build Settings")]
    [SerializeField] public int Village;
    [SerializeField] public int VillageHUD;
    [SerializeField] public int Credits;
    [SerializeField] public int Options;
    [SerializeField] public int StatUpgrade;
    [SerializeField] public int Dungeon;
    [SerializeField] public int DungeonHUD;
    [SerializeField] public int DetailsHUD;
    [SerializeField] public int LevelUp;
    [SerializeField] public int Pause;
    [SerializeField] public int Finish;
    [SerializeField] public int Lose;

    private void Awake()
    {
        // Destroy Instance when it is already existing, else create it
        if (Instance == null)
            Instance = (MenuManager)this;
        else if (Instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
