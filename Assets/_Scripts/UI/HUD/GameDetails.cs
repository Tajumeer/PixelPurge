using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Maya

/// <summary>
/// Show/Hide Game Details (current Spell Levels and Player Stats)
/// </summary>
public class GameDetails : MonoBehaviour
{
    [SerializeField] private GameObject ingameSpells;
    private int gameDetailsScene;

    private void Awake()
    {
        gameDetailsScene = MenuManager.Instance.DetailsHUD;
        Debug.Log("Scene: " + gameDetailsScene);
    }

    /// <summary>
    /// Loads the Scene with the Details and hides the ingame view of the spells
    /// </summary>
    public void ShowGameDetails()
    {
        if (ingameSpells != null) ingameSpells.SetActive(false);
        SceneManager.LoadScene(gameDetailsScene, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Unloads the Details Scene and shows the ingame spell view
    /// </summary>
    public void HideGameDetails()
    {
        if (ingameSpells != null) ingameSpells.SetActive(true);
        SceneManager.UnloadSceneAsync(gameDetailsScene);
    }
}
