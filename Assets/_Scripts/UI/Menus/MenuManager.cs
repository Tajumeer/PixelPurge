using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Script: Maya

[Serializable]
public enum Scenes
{
    Start = 0,
    UsernameInsertion,
    Village,
    Credits,
    Options,
    Shop,
    Scoreboard,
    LoadingScreen,
    Dungeon,
    DungeonHUD,
    DetailsHUD,
    LevelUp,
    Pause,
    Win,
    Death,
    Alpha,
    Beta
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

    /// <summary>
    /// Load All necessary Scenes for the Dungeon (Dungeon and HUD)
    /// </summary>
    public void LoadDungeon()
    {
        LoadSceneAsync(Scenes.LoadingScreen);
    }

    /// <summary>
    /// Unload all scenes that could be open in the Dungeon and Load Village
    /// </summary>
    public void UnloadDungeon(bool _loadVillage = true)
    {
        // check if pause menu, win screen, death screen, DetailsHUD was an open scene, then close it
        if (SceneManager.GetSceneByBuildIndex((int)Scenes.Pause).isLoaded) UnloadSceneAsync(Scenes.Pause);
        else if(SceneManager.GetSceneByBuildIndex((int)Scenes.Win).isLoaded) UnloadSceneAsync(Scenes.Win);
        else if(SceneManager.GetSceneByBuildIndex((int)Scenes.Death).isLoaded) UnloadSceneAsync(Scenes.Death);
        if (SceneManager.GetSceneByBuildIndex((int)Scenes.DetailsHUD).isLoaded) UnloadSceneAsync(Scenes.DetailsHUD);

        // unload all dungeon scenes
        UnloadSceneAsync(Scenes.Dungeon);
        UnloadSceneAsync(Scenes.DungeonHUD);

        // load village
        if (_loadVillage)
            LoadSceneAsync(Scenes.Village);

        Time.timeScale = 1f;
    }
}
