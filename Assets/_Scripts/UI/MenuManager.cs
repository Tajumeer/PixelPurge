using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script: Maya

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [Header("Scene Numbers in Build Settings")]
    [SerializeField] private int Village;
    [SerializeField] private int Credits;
    [SerializeField] private int Options;
    [SerializeField] private int StatUpgrade;
    [SerializeField] private int Pause;
    [SerializeField] private int LevelUp;
    [SerializeField] private int Finish;
    [SerializeField] private int Lose;

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
