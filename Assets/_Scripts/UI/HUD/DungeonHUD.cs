using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// Maya

public class DungeonHUD : MonoBehaviour
{
    private GameDetails m_detailsScript;
    private bool m_optionsLoaded;

    private void Start()
    {
        m_detailsScript = FindObjectOfType<GameDetails>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_optionsLoaded)
                UnloadOptions();
            else
                LoadOptions();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) LoadLevelUp();
        else if (Input.GetKeyDown(KeyCode.Alpha2)) UnloadLevelUp();
        else if (Input.GetKeyDown(KeyCode.Alpha3)) LoadPause();
        else if (Input.GetKeyDown(KeyCode.Alpha4)) UnloadPause();
    }

    #region Level Up

    public void LoadLevelUp()
    {
        Debug.Log("LevelUp");
        MenuManager.Instance.LoadSceneAsync(Scenes.LevelUp, LoadSceneMode.Additive);
        m_detailsScript.ShowGameDetails();
    }

    public void UnloadLevelUp()
    {
        m_detailsScript.HideGameDetails();
        MenuManager.Instance.UnloadSceneAsync(Scenes.LevelUp);
    }

    #endregion

    #region Pause

    public void LoadPause()
    {
        MenuManager.Instance.LoadSceneAsync(Scenes.Pause, LoadSceneMode.Additive);
        m_detailsScript.ShowGameDetails();
    }

    public void UnloadPause()
    {
        m_detailsScript.HideGameDetails();
        MenuManager.Instance.UnloadSceneAsync(Scenes.Pause);
    }

    #endregion

    #region Options

    private void LoadOptions()
    {
        m_optionsLoaded = true;
        MenuManager.Instance.LoadSceneAsync(Scenes.Options, LoadSceneMode.Additive);
        m_detailsScript.ShowGameDetails();
    }

    public void UnloadOptions()
    {
        m_detailsScript.HideGameDetails();
        MenuManager.Instance.UnloadSceneAsync(Scenes.Options);
        m_optionsLoaded = false;
    }

    #endregion
}
