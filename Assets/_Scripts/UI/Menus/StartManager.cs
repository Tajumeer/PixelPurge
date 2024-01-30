using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Maya

public class StartManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            MenuManager.Instance.LoadSceneAsync(Scenes.Beta);
            MenuManager.Instance.LoadSceneAsync(Scenes.DungeonHUD, LoadSceneMode.Additive);
        }
    }
}
