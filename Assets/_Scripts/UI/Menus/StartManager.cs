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
            Debug.Log("NAME: " + GameManager.Instance.UserName);
            if (GameManager.Instance.UserName == null || GameManager.Instance.UserName == "")
                MenuManager.Instance.LoadSceneAsync(Scenes.UsernameInsertion);
            else MenuManager.Instance.LoadVillage();
        }
    }
}
