using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDungeonManager : MonoBehaviour
{
    public void ReloadDungeon()
    {
        MenuManager.Instance.UnloadDungeon();
        MenuManager.Instance.LoadDungeon();
    }

    public void LoadVillage()
    {
        MenuManager.Instance.UnloadDungeon();
    }
}
