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
            if (GameManager.Instance.UserName == null || GameManager.Instance.UserName == "")
                MenuManager.Instance.LoadSceneAsync(Scenes.UsernameInsertion, CursorTypes.UI);
            else MenuManager.Instance.LoadSceneAsync(Scenes.Village, CursorTypes.None);
        }
    }
}
