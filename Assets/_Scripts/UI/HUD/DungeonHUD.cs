using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonHUD : MonoBehaviour
{
    private int sceneLevelUp;
    private int sceneDetails;
    private int scenePause;

    private GameDetails detailsScript;

    private void Start()
    {
        sceneLevelUp = MenuManager.Instance.LevelUp;
        sceneDetails = MenuManager.Instance.DetailsHUD;
        scenePause = MenuManager.Instance.Pause;

        detailsScript = FindObjectOfType<GameDetails>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) LoadLevelUp();
        if (Input.GetKeyDown(KeyCode.Alpha2)) UnloadLevelUp();
        if (Input.GetKeyDown(KeyCode.Alpha3)) LoadPause();
        if (Input.GetKeyDown(KeyCode.Alpha4)) UnloadPause();
    }

    #region Level Up

    public void LoadLevelUp()
    {
        Debug.Log("LevelUp");
        SceneManager.LoadScene(sceneLevelUp, LoadSceneMode.Additive);
        detailsScript.ShowGameDetails();
    }

    public void UnloadLevelUp()
    {
        detailsScript.HideGameDetails();
        SceneManager.UnloadSceneAsync(sceneLevelUp);
    }

    #endregion

    #region Pause

    public void LoadPause()
    {
        SceneManager.LoadScene(scenePause, LoadSceneMode.Additive);
        detailsScript.ShowGameDetails();
    }

    public void UnloadPause()
    {
        detailsScript.HideGameDetails();
        SceneManager.UnloadSceneAsync(scenePause);
    }

    #endregion
}
