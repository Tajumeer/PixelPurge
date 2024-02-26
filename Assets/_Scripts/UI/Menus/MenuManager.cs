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

public enum CursorTypes
{
    None,
    UI,
    Dungeon
}

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] private Texture2D m_cursorUITexture;
    [SerializeField] private Texture2D m_cursorDungeonTexture;

    private void Awake()
    {
        // Destroy Instance when it is already existing, else create it
        if (Instance == null)
            Instance = (MenuManager)this;
        else if (Instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void LoadSceneAsync(Scenes _scene, CursorTypes _cursor, LoadSceneMode _loadMode = LoadSceneMode.Single)
    {
        SetCursor(_cursor);

        SceneManager.LoadSceneAsync((int)_scene, _loadMode);
    }

    public void UnloadSceneAsync(Scenes _scene, CursorTypes _cursor)
    {
        SetCursor(_cursor);

        SceneManager.UnloadSceneAsync((int)_scene);
    }


    /// <summary>
    /// Load All necessary Scenes for the Dungeon (Dungeon and HUD)
    /// </summary>
    public void LoadDungeon()
    {
        LoadSceneAsync(Scenes.LoadingScreen, CursorTypes.None);
    }

    /// <summary>
    /// Unload all scenes that could be open in the Dungeon and Load Village
    /// </summary>
    public void UnloadDungeon(bool _loadVillage = true)
    {
        // check if pause menu, win screen, death screen, DetailsHUD was an open scene, then close it
        if (SceneManager.GetSceneByBuildIndex((int)Scenes.Pause).isLoaded) UnloadSceneAsync(Scenes.Pause, CursorTypes.Dungeon);
        else if(SceneManager.GetSceneByBuildIndex((int)Scenes.Win).isLoaded) UnloadSceneAsync(Scenes.Win, CursorTypes.Dungeon);
        else if(SceneManager.GetSceneByBuildIndex((int)Scenes.Death).isLoaded) UnloadSceneAsync(Scenes.Death, CursorTypes.Dungeon);
        if (SceneManager.GetSceneByBuildIndex((int)Scenes.DetailsHUD).isLoaded) UnloadSceneAsync(Scenes.DetailsHUD, CursorTypes.Dungeon);

        // unload all dungeon scenes
        UnloadSceneAsync(Scenes.Dungeon, CursorTypes.None);
        UnloadSceneAsync(Scenes.DungeonHUD, CursorTypes.None);

        // load village
        if (_loadVillage)
            LoadSceneAsync(Scenes.Village, CursorTypes.None);

        Time.timeScale = 1f;
    }

    private void SetCursor(CursorTypes _cursor)
    {
        switch (_cursor)
        {
            case CursorTypes.None:
                Cursor.visible = false;
                break;

            case CursorTypes.UI:
                Cursor.visible = true;
                Cursor.SetCursor(m_cursorUITexture, new Vector2(0f, 0f), CursorMode.Auto);
                break;

            case CursorTypes.Dungeon:
                Cursor.visible = true;
                Vector2 m_cursorPosition = new Vector2(m_cursorDungeonTexture.width / 2, m_cursorDungeonTexture.height / 2);
                Cursor.SetCursor(m_cursorDungeonTexture, m_cursorPosition, CursorMode.Auto);
                break;
        }
    }
}
