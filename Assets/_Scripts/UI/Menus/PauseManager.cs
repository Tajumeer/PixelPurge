using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_exitButtonText;

    private void OnEnable()
    {
        if (SceneManager.GetSceneByBuildIndex((int)Scenes.Village).isLoaded) m_exitButtonText.text = "Exit";
        else m_exitButtonText.text = "Village";
    }

    public void LoadOptions()
    {
        MenuManager.Instance.LoadSceneAsync(Scenes.Options, CursorTypes.UI, LoadSceneMode.Additive);
    }

    public void UnloadPause()
    {
        // if we are in the dungeon
        if (SceneManager.GetSceneByBuildIndex((int)Scenes.Dungeon).isLoaded)
            FindObjectOfType<DungeonHUDManager>().UnloadPause();

        // or in the village
        else
        {
            MenuManager.Instance.UnloadSceneAsync(Scenes.Pause, CursorTypes.None);  // from Pause to Village
            Time.timeScale = 1f;
        }
    }

    public void Exit()
    {
        // if we are in the dungeon, go back to village
        if (SceneManager.GetSceneByBuildIndex((int)Scenes.Dungeon).isLoaded)
            MenuManager.Instance.UnloadDungeon();

        // else quit game
        else
        {
#if UNITY_STANDALONE
            Application.Quit();
#endif

#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
        }
    }
}
