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

    private void OnEnable()
    {
        RandomizeSpellsToShow(GetAvailableSpells());
    }

    /// <summary>
    /// Go through every active and passive Spell and add it to the list if its not on max level
    /// </summary>
    /// <returns></returns>
    private List<Spells> GetAvailableSpells()
    {
        List<Spells> availableSpells = new List<Spells>();

        // check if active spells are full

        for (int i = 0; i < (int)Spells.ActiveSpells; i++)
        {
            // if at this place is no spell skip
            if (m_dataSpells.activeSpellSO[i] == null) continue;

            // if spell is already at max level, its not available for choosing
            if (m_dataSpells.activeSpellSO[i].Level >= m_dataSpells.activeSpellSO[i].MaxLevel) continue;

            availableSpells.Add((Spells)i);
        }

        // check if passive spells are full

        for (int i = (int)Spells.ActiveSpells + 1; i < (int)Spells.PassiveSpells; i++)
        {
            // get the index of the passive spell ("delete" active Spells for index)
            int idx = i - ((int)Spells.ActiveSpells + 1);

            // if at this place is no spell skip
            if (m_dataSpells.passiveSpellSO[idx] == null) continue;

            // if spell is already at max level, its not available for choosing
            if (m_dataSpells.passiveSpellSO[idx].Level >= m_dataSpells.passiveSpellSO[idx].MaxLevel) continue;

            availableSpells.Add((Spells)i);
        }

        return availableSpells;
    }

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

        // create new GameObject
        GameObject spellCard = Instantiate(m_prefab_spellCardPassive);

        spellCard.transform.SetParent(m_spellCardParent);

        // set spell, icon, name
        ChooseSpell values = spellCard.GetComponent<ChooseSpell>();
        values.m_spell = _spell;
        values.m_icon.sprite = m_dataSpells.passiveSpellSO[idx].SpellIcon;
        values.m_name.text = m_dataSpells.passiveSpellSO[idx].SpellName;

        // if its not already learned, show spell description
        if (m_dataSpells.passiveSpellSO[idx].Level == 0 && m_dataSpells.passiveSpellSO[idx].MaxLevel != 0)
        {
            values.m_description.alignment = TMPro.TextAlignmentOptions.Center;
            values.m_description.text = m_dataSpells.passiveSpellSO[idx].SpellDescription;
        }
        // else show upgrades
        else
        {
            values.m_description.text = "Upgrades incoming";
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
}
