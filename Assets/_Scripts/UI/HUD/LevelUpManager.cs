using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class LevelUpManager : MonoBehaviour
{
    [SerializeField] private SO_AllSpells m_dataSpells;

    [SerializeField] private GameObject m_prefab_spellCardActive;
    [SerializeField] private GameObject m_prefab_spellCardPassive;
    [SerializeField] private GameObject m_prefab_spellCardGold;
    [SerializeField] private Transform m_spellCardParent;

    [Tooltip("How many Active / passive spells can be learned in one run")]
    [SerializeField] private int m_spellSlots = 4;

    private void OnEnable()
    {
        RandomizeSpellsToShow(GetAvailableSpells());
    }

    #region GetAvailableSpells

    /// <summary>
    /// Makes a List of all available Spells to learn/upgrade 
    /// (dont show new active or passive spells when there are no free slots)
    /// </summary>
    private List<Spells> GetAvailableSpells()
    {
        List<Spells> availableSpells = new List<Spells>();

        // lists of all active/passive spells that can be learned or upgraded
        (List<Spells> availablePassiveSpells, bool newPassiveSpells) = GetAvailablePassiveSpells();
        (List<Spells> availableActiveSpells, bool newActiveSpells) = GetAvailableActiveSpells();


        // only add active spells to the list when there are free slots for active spells
        if (newActiveSpells)
        {
            foreach (Spells spell in availableActiveSpells)
            {
                availableSpells.Add(spell);
            }
        }

        // only add passive spells to the list when there are free slots for passive spells
        if (newPassiveSpells)
        {
            foreach (Spells spell in availablePassiveSpells)
            {
                availableSpells.Add(spell);
            }
        }

        return availableSpells;
    }

    /// <summary>
    /// Go through every activeSpell and add it to the list if its not on max level
    /// </summary>
    /// <returns>List of all available Active Spells || true: active spell slots are not full</returns>
    private (List<Spells>, bool) GetAvailableActiveSpells()
    {
        List<Spells> availableActiveSpells = new List<Spells>();

        int takenActiveSpells = 0;

        for (int i = 0; i < (int)Spells.ActiveSpells; i++)
        {
            // if at this place is no spell skip
            if (m_dataSpells.activeSpellSO[i] == null) continue;

            // if spell is already at max level, its not available for choosing
            if (m_dataSpells.activeSpellSO[i].Level >= m_dataSpells.activeSpellSO[i].MaxLevel) continue;

            // count how many spells are already learned (> level 0)
            if (m_dataSpells.activeSpellSO[i].Level > 0) takenActiveSpells++;

            availableActiveSpells.Add((Spells)i);
        }

        return (availableActiveSpells, takenActiveSpells < m_spellSlots);
    }

    /// <summary>
    /// Go through every passive Spell and add it to the list if its not on max level
    /// </summary>
    /// <returns>List of all available Passive Spells || true: passive spell slots are not full</returns>
    private (List<Spells>, bool) GetAvailablePassiveSpells()
    {
        List<Spells> availablePassiveSpells = new List<Spells>();

        int takenPassiveSpells = 0;

        for (int i = (int)Spells.ActiveSpells + 1; i < (int)Spells.PassiveSpells; i++)
        {
            // get the index of the passive spell ("delete" active Spells for index)
            int idx = i - ((int)Spells.ActiveSpells + 1);

            // if at this place is no spell skip
            if (m_dataSpells.passiveSpellSO[idx] == null) continue;

            // if spell is already at max level, its not available for choosing
            if (m_dataSpells.passiveSpellSO[idx].Level >= m_dataSpells.passiveSpellSO[idx].MaxLevel) continue;

            // count how many spells are already learned (> level 0)
            if (m_dataSpells.passiveSpellSO[idx].Level > 0) takenPassiveSpells++;

            availablePassiveSpells.Add((Spells)i);
        }

        return (availablePassiveSpells, takenPassiveSpells < m_spellSlots);
    }

    #endregion

    /// <summary>
    /// Random choose 3 Spells from the List and show them in the UI
    /// </summary>
    /// <param name="_availableSpells"></param>
    private void RandomizeSpellsToShow(List<Spells> _availableSpells)
    {
        // choose 3 spells from List
        for (int i = 0; i < 3; i++)
        {
            // if there are no spell (upgrades) left, make a gold card
            if (_availableSpells.Count == 0)
            {
                CardGold();
                continue;
            }

            // choose a random Spell from all available Spells List
            int randomSpell = Random.Range(0, _availableSpells.Count);

            Spells chosenSpell = _availableSpells[randomSpell];

            // and put it in the HUD (check if its an active or passive spell)
            if ((int)chosenSpell < (int)Spells.ActiveSpells)
                CardActiveSpell(chosenSpell);
            else if ((int)chosenSpell > (int)Spells.ActiveSpells && (int)chosenSpell < (int)Spells.PassiveSpells)
                CardPassiveSpell(chosenSpell);

            // Remove this spell from list to avoid showing it multiple times
            _availableSpells.RemoveAt(randomSpell);  // remove spell from list because its already in the HUD
        }
    }

    #region Cards

    /// <summary>
    /// Creates a Active Spell Card from the Prefab and set the values to the specific spell (icon, name, description, etc.)
    /// </summary>
    /// <param name="_spell"></param>
    private void CardActiveSpell(Spells _spell)
    {
        // create new GameObject
        GameObject spellCard = Instantiate(m_prefab_spellCardActive);

        spellCard.transform.SetParent(m_spellCardParent);

        // set spell, icon, name
        ChooseSpell values = spellCard.GetComponent<ChooseSpell>();
        values.m_spell = _spell;
        values.m_icon.sprite = m_dataSpells.activeSpellSO[(int)_spell].SpellIcon;
        values.m_name.text = m_dataSpells.activeSpellSO[(int)_spell].SpellName;

        // if its not already learned, show spell description
        if (m_dataSpells.activeSpellSO[(int)_spell].Level == 0 && m_dataSpells.activeSpellSO[(int)_spell].MaxLevel != 0)
        {
            values.m_description.alignment = TMPro.TextAlignmentOptions.Center;
            values.m_description.text = m_dataSpells.activeSpellSO[(int)_spell].SpellDescription;
        }
        // else show upgrades
        else
        {
            values.m_description.text = "Upgrades incoming";
        }
    }

    /// <summary>
    /// Creates a Passive Spell Card from the Prefab and set the values to the specific spell (icon, name, description, etc.)
    /// </summary>
    /// <param name="_spell"></param>
    private void CardPassiveSpell(Spells _spell)
    {
        // get the index of the passive spell ("delete" active Spells for index)
        int idx = (int)_spell - ((int)Spells.ActiveSpells + 1);

        // get the Scriptable Object of this spell
        SO_PassiveSpells spellSO = m_dataSpells.passiveSpellSO[idx];

        // create new GameObject
        GameObject spellCard = Instantiate(m_prefab_spellCardPassive);
        spellCard.transform.SetParent(m_spellCardParent);

        // set spell, icon, name
        ChooseSpell values = spellCard.GetComponent<ChooseSpell>();
        values.m_spell = _spell;
        values.m_icon.sprite = spellSO.SpellIcon;
        values.m_name.text = spellSO.SpellName;

        // if its not already learned, show spell description
        if (spellSO.Level == 0 && spellSO.MaxLevel != 0)
        {
            values.m_description.alignment = TMPro.TextAlignmentOptions.Center;
            values.m_description.text = spellSO.SpellDescription;
        }
        // else show upgrades
        else
        {
            // if Stat is a positive integer -> is is no % but its value is added (e.g. 20)
            if(spellSO.Stat[spellSO.Level] >= 1 && spellSO.Stat[spellSO.Level] % 1 == 0)
            {
                values.m_description.text = "+ " + spellSO.Stat[spellSO.Level] + " " + spellSO.SpellUpgradeDescription;
            }
            // if Stat is greater than 1 but is a decimal number -> it is a % value (e.g. 1.4)
            else if(spellSO.Stat[spellSO.Level] > 1 && spellSO.Stat[spellSO.Level] % 1 != 0)
            {
                float percent = (spellSO.Stat[spellSO.Level] - 1) * 100;
                values.m_description.text = "+ " + percent + "% " + spellSO.SpellUpgradeDescription;
            }
            // if Stat is between 0 and 1 -> it is a % value (e.g. 0.6)
            else if(spellSO.Stat[spellSO.Level] < 1 && spellSO.Stat[spellSO.Level] > 0)
            {
                float percent = (1 - spellSO.Stat[spellSO.Level]) * 100;
                values.m_description.text = "+ " + percent + "% " + spellSO.SpellUpgradeDescription;
            }
            else
                values.m_description.text = "Hidden Upgrade :)";
        }
    }

    /// <summary>
    /// Create a Gold Card from the Prefab
    /// </summary>
    private void CardGold()
    {
        // create new GameObject
        GameObject spellCard = Instantiate(m_prefab_spellCardGold);
        spellCard.transform.SetParent(m_spellCardParent);
    }

    #endregion
}
