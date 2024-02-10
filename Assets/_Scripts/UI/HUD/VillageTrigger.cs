using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageTrigger : MonoBehaviour
{
    [SerializeField] private Scenes m_sceneToLoad;
    [SerializeField] private GameObject m_button;
    private bool m_canLoadScene = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        m_canLoadScene = true;
        if (m_button != null) m_button.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        m_canLoadScene = false;
        if (m_button != null) m_button.SetActive(false);
    }

    private void Update()
    {
        if (m_canLoadScene)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // cannot open pausemenu if optionsmenu or pause are open or the scene is already loaded
                if (SceneManager.GetSceneByBuildIndex((int)m_sceneToLoad).isLoaded) UnloadScene();
                else if (SceneManager.GetSceneByBuildIndex((int)Scenes.Pause).isLoaded) return;
                else if (SceneManager.GetSceneByBuildIndex((int)Scenes.Options).isLoaded) return;
                else LoadScene();
            }
        }
    }


    /// <summary>
    /// Load the Scene (Dungeon or an additive scene to the village)
    /// </summary>
    private void LoadScene()
    {
        if (m_sceneToLoad == Scenes.Dungeon)     // Load Dungeon
            MenuManager.Instance.LoadDungeon();
        else                                    // Load Credits, Shop or Scoreboard
        {
            Time.timeScale = 0f;
            MenuManager.Instance.LoadSceneAsync(m_sceneToLoad, LoadSceneMode.Additive);
        }
    }

    /// <summary>
    /// Unload Scene
    /// </summary>
    private void UnloadScene()
    {
        //can only be called when village is still open in the background, so we dont need to check if we are in the dungeon

        MenuManager.Instance.UnloadSceneAsync(m_sceneToLoad);
        Time.timeScale = 1f;
    }
}
