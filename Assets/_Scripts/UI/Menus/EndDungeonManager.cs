using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDungeonManager : MonoBehaviour
{
    public void ReloadDungeon()
    {
        MenuManager.Instance.UnloadDungeon(false);
        MenuManager.Instance.LoadDungeon();
        Time.timeScale = 1f;
    }

    public void LoadVillage()
    {
        MenuManager.Instance.UnloadDungeon();
        Time.timeScale = 1f;
    }
}
