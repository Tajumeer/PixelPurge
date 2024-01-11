using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

/// <summary>
/// Enable/Disable the GameObjects with the Informations about Spell Level and Player Stats, depending on own visibility
/// </summary>
public class SpellAndStatDetails : MonoBehaviour
{
    [SerializeField] private GameObject spellLevelActive;
    [SerializeField] private GameObject spellLevelPassive;
    [SerializeField] private GameObject stats;

    private void OnEnable()
    {
        if (spellLevelActive != null) spellLevelActive.SetActive(true);
        if (spellLevelPassive != null) spellLevelPassive.SetActive(true);
        if (stats != null) stats.SetActive(true);
    }

    private void OnDisable()
    {
        if (spellLevelActive != null) spellLevelActive.SetActive(false);
        if (spellLevelPassive != null) spellLevelPassive.SetActive(false);
        if (stats != null) stats.SetActive(false);
    }
}
