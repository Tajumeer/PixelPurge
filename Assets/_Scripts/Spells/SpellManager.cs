using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Maya

public class SpellManager : MonoBehaviour
{
    // spell lernen
    // upgrade
    // UI

    SpellSpawner spawnScript;

    [Header("Scriptable Objects")]
    [SerializeField] private SO_SpellUpgrades data_Upgrades;
    [SerializeField] private SO_Spells data_AllDirections;
    [SerializeField] private SO_Spells data_NearPlayer;

    [Header("Spell Levels")]
    private int level_AllDirections = 1;
    private int level_NearPlayer = 1;

    private void OnEnable()
    {
        spawnScript = FindObjectOfType<SpellSpawner>();
        //LearnAllDirections();
        LearnNearPlayer();
    }

    #region Learn New Spells

    /// <summary>
    /// Learn the Spell "AllDirections" and show it in UI
    /// </summary>
    public void LearnAllDirections()
    {
        level_AllDirections = 1;
        spawnScript.data_AllDirections = Instantiate(data_AllDirections);
        spawnScript.active_AllDirections = true;
        
        // UI
    }

    /// <summary>
    /// Learn the Spell "NearPlayer" and show it in UI
    /// </summary>
    public void LearnNearPlayer()
    {
        level_NearPlayer = 1;
        spawnScript.data_NearPlayer = Instantiate(data_NearPlayer);
        spawnScript.active_NearPlayer = true;

        // UI
    }

    #endregion

    #region Upgrade Spells

    /// <summary>
    /// Upgrade values of the spell and update UI
    /// </summary>
    public void UpradeAllDirections()
    {
        spawnScript.data_AllDirections.damage *= (1f + data_Upgrades.Damage[level_AllDirections]);

        level_AllDirections++;

        // UI
    }

    /// <summary>
    /// Upgrade values of the spell and update UI
    /// </summary>
    public void UpgradeNearPlayer()
    {
        spawnScript.data_NearPlayer.damage *= (1f + data_Upgrades.Damage[level_NearPlayer]);

        level_NearPlayer++;

        // UI
    }

    #endregion
}
