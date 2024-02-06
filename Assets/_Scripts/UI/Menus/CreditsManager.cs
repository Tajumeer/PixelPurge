using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class CreditsManager : MonoBehaviour
{
    public void UnloadScene()
    {
        MenuManager.Instance.UnloadSceneAsync(Scenes.Credits);
    }
}
