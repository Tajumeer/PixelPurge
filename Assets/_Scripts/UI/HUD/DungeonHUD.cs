using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// Maya

public class DungeonHUD : MonoBehaviour
{
    [SerializeField] private GameObject m_ingameSpells;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // cannot open pausemenu if optionsmenu, win or death screen are open
            if (SceneManager.GetSceneByBuildIndex((int)Scenes.Options).isLoaded) return;
            else if (SceneManager.GetSceneByBuildIndex((int)Scenes.Pause).isLoaded) UnloadPause();
            else if (SceneManager.GetSceneByBuildIndex((int)Scenes.Win).isLoaded) return;
            else if (SceneManager.GetSceneByBuildIndex((int)Scenes.Death).isLoaded) return;
            else LoadPause();
        }

        // Debug
        if (Input.GetKeyDown(KeyCode.Alpha1)) LoadLevelUp();
        else if (Input.GetKeyDown(KeyCode.Alpha2)) UnloadLevelUp();
        else if (Input.GetKeyDown(KeyCode.Alpha3)) LoadPause();
        else if (Input.GetKeyDown(KeyCode.Alpha4)) UnloadPause();
    }

    #region Details HUD

    /// <summary>
    /// Loads the Scene with the Details and hides the ingame view of the spells
    /// </summary>
    private void ShowGameDetails()
    {
        if (m_ingameSpells != null) m_ingameSpells.SetActive(false);
        MenuManager.Instance.LoadSceneAsync(Scenes.DetailsHUD, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Unloads the Details Scene and shows the ingame spell view
    /// </summary>
    private void HideGameDetails()
    {
        if (m_ingameSpells != null) m_ingameSpells.SetActive(true);
        MenuManager.Instance.UnloadSceneAsync(Scenes.DetailsHUD);
    }

    #endregion

    #region Level Up

    public void LoadLevelUp()
    {
        Debug.Log("LevelUp");
        ShowGameDetails();
        MenuManager.Instance.LoadSceneAsync(Scenes.LevelUp, LoadSceneMode.Additive);
    }

    public void UnloadLevelUp()
    {
        MenuManager.Instance.UnloadSceneAsync(Scenes.LevelUp);
        HideGameDetails();
    }

    #endregion

    #region Pause

    private void LoadPause()
    {
        ShowGameDetails();
        MenuManager.Instance.LoadSceneAsync(Scenes.Pause, LoadSceneMode.Additive);
    }

    public void UnloadPause()
    {
        MenuManager.Instance.UnloadSceneAsync(Scenes.Pause);
        HideGameDetails();
    }

    #endregion
}
