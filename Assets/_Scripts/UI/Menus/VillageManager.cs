using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Maya

public class VillageManager : MonoBehaviour
{
    private void Update()
    {
        // Open/Close Pausemenu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // cannot open pausemenu if options, credits, scoreboard or shop are open
            if (SceneManager.GetSceneByBuildIndex((int)Scenes.Options).isLoaded) return;
            else if (SceneManager.GetSceneByBuildIndex((int)Scenes.Pause).isLoaded) UnloadPause();
            else if (SceneManager.GetSceneByBuildIndex((int)Scenes.Credits).isLoaded) return;
            else if (SceneManager.GetSceneByBuildIndex((int)Scenes.Scoreboard).isLoaded) return;
            else if (SceneManager.GetSceneByBuildIndex((int)Scenes.Shop).isLoaded) return;
            else LoadPause();
        }
    }

    private void LoadPause()
    {
        Time.timeScale = 0f;
        MenuManager.Instance.LoadSceneAsync(Scenes.Pause, LoadSceneMode.Additive);
    }

    public void UnloadPause()
    {
        MenuManager.Instance.UnloadSceneAsync(Scenes.Pause);
        Time.timeScale = 1f;
    }
}
