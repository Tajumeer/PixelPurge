using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public void LoadOptions()
    {
        MenuManager.Instance.LoadSceneAsync(Scenes.Options, CursorTypes.UI, LoadSceneMode.Additive);
    }

    public void UnloadPause()
    {
        // if we are in the dungeon
        if (FindObjectOfType<DungeonHUDManager>() != null)
            FindObjectOfType<DungeonHUDManager>().UnloadPause();

        // or in the village
        else
        {
            MenuManager.Instance.UnloadSceneAsync(Scenes.Pause, CursorTypes.None);  // from Pause to Village
            Time.timeScale = 1f;
        }
    }

    public void UnloadDungeon()
    {
        MenuManager.Instance.UnloadDungeon();
    }
}
