using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Script: Maya

public enum Scenes
{
    Start = 0,
    UsernameInsertion,
    Village,
    VillageHUD,
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

    public void LoadDungeon()
    {
        LoadSceneAsync(Scenes.Dungeon);
        LoadSceneAsync(Scenes.DungeonHUD, LoadSceneMode.Additive);
    }

    public void UnloadDungeon()
    {
        // unload all scenes
        UnloadSceneAsync(Scenes.Dungeon);
        UnloadSceneAsync(Scenes.DungeonHUD);
        // check if pause menu/win screen/death screen was an open scene, then close it
        if (SceneManager.GetSceneByBuildIndex((int)Scenes.Pause).isLoaded) UnloadSceneAsync(Scenes.Pause);
        else if(SceneManager.GetSceneByBuildIndex((int)Scenes.Win).isLoaded) UnloadSceneAsync(Scenes.Win);
        else if(SceneManager.GetSceneByBuildIndex((int)Scenes.Death).isLoaded) UnloadSceneAsync(Scenes.Death);

        // load all scenes for the village
        LoadSceneAsync(Scenes.Village);
        LoadSceneAsync(Scenes.VillageHUD, LoadSceneMode.Additive);
    }
}
