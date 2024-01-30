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
        List<Spells> availableSpells = new List<Spells>();

        // fill List with all Spells that are not on maxLevel
        for (int i = 0; i < (int)Spells.SpellAmount; i++)
        {   
            // if at this place is no spell skip
            if (m_dataSpells.spellSO[i] == null) continue;

            // if spell is already at max level, its not available for choosing
            if (m_dataSpells.spellSO[i].Level >= m_dataSpells.spellSO[i].MaxLevel) continue;

            availableSpells.Add((Spells)i);
        }

        // choose 3 spells from List
        for (int i = 0; i < 3; i++)
        {
            // if there are no spell (upgrades) left, make a gold card
            if (availableSpells.Count == 0)
            {
                CardGold();
                continue;
            }

            // choose a random Spell from all available Spells List
            int randomSpell = Random.Range(0, availableSpells.Count);

            // and put it in the HUD
            SpellCard(availableSpells[randomSpell]);

            availableSpells.RemoveAt(randomSpell);  // remove spell from list because its already in the HUD
        }
    }

    private void SpellCard(Spells _spell)
    {
        // create new GameObject
        GameObject spellCard;

        // check if its an active or passive spell
        if ((int)_spell < (int)Spells.ActiveSpells)
            spellCard = Instantiate(m_prefab_spellCardActive);
        else if ((int)_spell > (int)Spells.ActiveSpells && (int)_spell < (int)Spells.PassiveSpells)
            spellCard = Instantiate(m_prefab_spellCardPassive);
        else return;

        spellCard.transform.SetParent(m_spellCardParent);

        // set spell, icon, name
        ChooseSpell values = spellCard.GetComponent<ChooseSpell>();
        values.m_spell = _spell;
        values.m_icon.sprite = m_dataSpells.spellSO[(int)_spell].SpellIcon;
        values.m_name.text = m_dataSpells.spellSO[(int)_spell].SpellName;

        // if its not already learned, show spell description
        if (m_dataSpells.spellSO[(int)_spell].Level == 0 && m_dataSpells.spellSO[(int)_spell].MaxLevel != 0)
        {
            values.m_description.alignment = TMPro.TextAlignmentOptions.Center;
            values.m_description.text = m_dataSpells.spellSO[(int)_spell].SpellDescription;
        }
        // else show upgrades
        else
        {
            values.m_description.text = "Upgrades incoming";
        }
    }
    private void CardGold()
    {
        // create new GameObject
        GameObject spellCard = Instantiate(m_prefab_spellCardGold);
        spellCard.transform.SetParent(m_spellCardParent);
    }
}
