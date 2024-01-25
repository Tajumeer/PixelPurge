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
    [SerializeField] private GameObject m_ingameSpells;

    /// <summary>
    /// Loads the Scene with the Details and hides the ingame view of the spells
    /// </summary>
    public void ShowGameDetails()
    {
        if (m_ingameSpells != null) m_ingameSpells.SetActive(false);
        MenuManager.Instance.LoadSceneAsync(Scenes.DetailsHUD, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Unloads the Details Scene and shows the ingame spell view
    /// </summary>
    public void HideGameDetails()
    {
        if (m_ingameSpells != null) m_ingameSpells.SetActive(true);
        MenuManager.Instance.UnloadSceneAsync(Scenes.DetailsHUD);
    }
}
