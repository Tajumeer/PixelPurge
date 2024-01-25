using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Script: Maya

public enum Scenes
{
    Start = 0,
    Village,
    VillageHUD,
    Credits,
    Options,
    StatUpgrade,
    Dungeon,
    DungeonHUD,
    DetailsHUD,
    LevelUp,
    Pause,
    Finish,
    Lose,
    Alpha
}

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    private void Awake()
    {
        // Destroy Instance when it is already existing, else create it
        if (Instance == null)
            Instance = (MenuManager)this;
        else if (Instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void LoadSceneAsync(Scenes scene, LoadSceneMode mode = LoadSceneMode.Single)
    {
        SceneManager.LoadSceneAsync((int)scene, mode);
    }

    public void UnloadSceneAsync(Scenes scene)
    {
        SceneManager.UnloadSceneAsync((int)scene);
    }
}
