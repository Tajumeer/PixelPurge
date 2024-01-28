using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public void LoadOptions()
    {
        MenuManager.Instance.LoadSceneAsync(Scenes.Options, LoadSceneMode.Additive);
    }

    public void UnloadPause()
    {
        FindObjectOfType<DungeonHUDManager>().UnloadPause();
    }

    public void UnloadDungeon()
    {
        MenuManager.Instance.UnloadDungeon();
    }
}
