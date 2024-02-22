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
            // open pausemenu or close credits, scoreboard, shop
            if (SceneManager.GetSceneByBuildIndex((int)Scenes.Options).isLoaded) return;
            else if (SceneManager.GetSceneByBuildIndex((int)Scenes.Pause).isLoaded) UnloadScene(Scenes.Pause);
            else if (SceneManager.GetSceneByBuildIndex((int)Scenes.Credits).isLoaded) UnloadScene(Scenes.Credits);
            else if (SceneManager.GetSceneByBuildIndex((int)Scenes.Scoreboard).isLoaded) UnloadScene(Scenes.Scoreboard);
            else if (SceneManager.GetSceneByBuildIndex((int)Scenes.Shop).isLoaded) UnloadScene(Scenes.Shop);
            else LoadPause();
        }
    }

    private void LoadPause()
    {
        Time.timeScale = 0f;
        MenuManager.Instance.LoadSceneAsync(Scenes.Pause, LoadSceneMode.Additive);
    }

    public void UnloadScene(Scenes _scene)
    {
        MenuManager.Instance.UnloadSceneAsync(_scene);
        Time.timeScale = 1f;
    }
}
