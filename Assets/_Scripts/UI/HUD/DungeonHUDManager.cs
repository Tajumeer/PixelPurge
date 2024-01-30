using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Maya

public class DungeonHUDManager : MonoBehaviour
{
    [SerializeField] private GameObject m_ingameSpells;
    [Space]
    [SerializeField] private Image m_xpBar;

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
    }

    #region Details HUD

    /// <summary>
    /// Loads the Scene with the Details and hides the ingame view of the spells
    /// </summary>
    private void ShowGameDetails()
    {
        // do nothing if its already loaded (e.g. open pause in levelup screen)
        if (SceneManager.GetSceneByBuildIndex((int)Scenes.DetailsHUD).isLoaded) return;

        Time.timeScale = 0f;
        if (m_ingameSpells != null) m_ingameSpells.SetActive(false);
        MenuManager.Instance.LoadSceneAsync(Scenes.DetailsHUD, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Unloads the Details Scene and shows the ingame spell view
    /// </summary>
    private void HideGameDetails()
    {
        // if levelUp screen is still open, dont close details (e.g. if pause is closed but levelup is still open)
        if (SceneManager.GetSceneByBuildIndex((int)Scenes.LevelUp).isLoaded) return;

        if (m_ingameSpells != null) m_ingameSpells.SetActive(true);
        MenuManager.Instance.UnloadSceneAsync(Scenes.DetailsHUD);
        Time.timeScale = 1f;
    }

    #endregion

    #region Level and XP

    public void ShowXP(float _currentXP, float _neededXP)
    {
        m_xpBar.fillAmount = _currentXP / _neededXP;
    }

    public void LoadLevelUp()
    {
        Time.timeScale = 0f;

        ShowGameDetails();
        MenuManager.Instance.LoadSceneAsync(Scenes.LevelUp, LoadSceneMode.Additive);
    }

    public void UnloadLevelUp()
    {
        MenuManager.Instance.UnloadSceneAsync(Scenes.LevelUp);
        HideGameDetails();

        Time.timeScale = 1f;
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
