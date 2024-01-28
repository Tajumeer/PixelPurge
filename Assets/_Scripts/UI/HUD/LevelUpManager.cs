using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class LevelUpManager : MonoBehaviour
{
    private SpellManager m_spellScript;

    [SerializeField] private GameObject m_prefab_spellCardActive;
    [SerializeField] private GameObject m_prefab_spellCardPassive;
    [SerializeField] private Transform m_spellCardParent;
    [Space]
    [SerializeField] private Sprite m_goldIcon;

    private void Awake()
    {
        m_spellScript = FindObjectOfType<SpellManager>();
    }

    private void OnEnable()
    {
        List<Spells> availableSpells = new List<Spells>();

        // fill List with all Spells that are not on maxLevel
        for (int i = 0; i < (int)Spells.SpellAmount; i++)
        {
            // if spell is already at max level, its not available for choosing
            if (m_spellScript.m_level[i] >= m_spellScript.m_maxSpellLevel) return;

            availableSpells.Add((Spells)i);
        }

        // choose 3 spells from List
        for (int i = 0; i < 3; i++)
        {
            if (availableSpells.Count == 0) CardGold();

            // choose a random Spell from all available Spells List
            int randomSpell = Random.Range(0, availableSpells.Count);

            // and put it in the HUD
            switch (availableSpells[randomSpell])
            {
                case Spells.AllDirections:
                    CardAllDirections();
                    break;

                case Spells.Aura:
                    CardAura();
                    break;

                case Spells.BaseArcher:
                    CardBaseArcher();
                    break;

                case Spells.NearPlayer:
                    CardNearPlayer();
                    break;

                default:
                    CardGold();
                    break;
            }

            availableSpells.RemoveAt(randomSpell);  // remove spell from list because its already in the HUD
        }
    }

    private void CardGold()
    {
        // create new GameObject
        GameObject spellCard = Instantiate(m_prefab_spellCardActive);
        spellCard.transform.SetParent(m_spellCardParent);

        // set spell, icon, name and description
        ChooseSpell values = spellCard.GetComponent<ChooseSpell>();
        values.m_spell = Spells.SpellAmount;
        values.m_icon.sprite = m_goldIcon;
        values.m_name.text = "Gold";

        values.m_description.alignment = TMPro.TextAlignmentOptions.Center;
        values.m_description.text = "So much gold, i´m gonna get rich!";
    }

    private void CardBaseArcher()
    {
        // create new GameObject
        GameObject spellCard = Instantiate(m_prefab_spellCardActive);
        spellCard.transform.SetParent(m_spellCardParent);

        // set spell, icon, name
        ChooseSpell values = spellCard.GetComponent<ChooseSpell>();
        values.m_spell = Spells.BaseArcher;
        values.m_icon.sprite = m_spellScript.m_data_BaseArcher.SpellIcon;
        values.m_name.text = m_spellScript.m_data_BaseArcher.SpellName;

        // if its not already learned, show spell description
        if (m_spellScript.m_level[(int)Spells.BaseArcher] == 0)
        {
            values.m_description.alignment = TMPro.TextAlignmentOptions.Center;
            values.m_description.text = m_spellScript.m_data_BaseArcher.SpellDescription;
        }
        // else show upgrades
        else
        {
            values.m_description.text = "Upgrade";
        }
    }

    private void CardAllDirections()
    {
        // create new GameObject
        GameObject spellCard = Instantiate(m_prefab_spellCardActive);
        spellCard.transform.SetParent(m_spellCardParent);

        // set spell, icon, name
        ChooseSpell values = spellCard.GetComponent<ChooseSpell>();
        values.m_spell = Spells.AllDirections;
        values.m_icon.sprite = m_spellScript.m_data_AllDirections.SpellIcon;
        values.m_name.text = m_spellScript.m_data_AllDirections.SpellName;

        // if its not already learned, show spell description
        if (m_spellScript.m_level[(int)Spells.AllDirections] == 0)
        {
            values.m_description.alignment = TMPro.TextAlignmentOptions.Center;
            values.m_description.text = m_spellScript.m_data_AllDirections.SpellDescription;
        }
        // else show upgrades
        else
        {
            values.m_description.text = "Upgrade";
        }
    }

    private void CardNearPlayer()
    {
        // create new GameObject
        GameObject spellCard = Instantiate(m_prefab_spellCardActive);
        spellCard.transform.SetParent(m_spellCardParent);

        // set spell, icon, name
        ChooseSpell values = spellCard.GetComponent<ChooseSpell>();
        
        values.m_spell = Spells.NearPlayer;
        values.m_icon.sprite = m_spellScript.m_data_NearPlayer.SpellIcon;
        values.m_name.text = m_spellScript.m_data_NearPlayer.SpellName;

        // if its not already learned, show spell description
        if (m_spellScript.m_level[(int)Spells.NearPlayer] == 0)
        {
            values.m_description.alignment = TMPro.TextAlignmentOptions.Center;
            values.m_description.text = m_spellScript.m_data_NearPlayer.SpellDescription;
        }
        // else show upgrades
        else
        {
            values.m_description.text = "Upgrade";
        }
    }

    private void CardAura()
    {
        // create new GameObject
        GameObject spellCard = Instantiate(m_prefab_spellCardActive);
        spellCard.transform.SetParent(m_spellCardParent);

        // set spell, icon, name
        ChooseSpell values = spellCard.GetComponent<ChooseSpell>();
        values.m_spell = Spells.Aura;
        values.m_icon.sprite = m_spellScript.m_data_Aura.SpellIcon;
        values.m_name.text = m_spellScript.m_data_Aura.SpellName;

        // if its not already learned, show spell description
        if (m_spellScript.m_level[(int)Spells.Aura] == 0)
        {
            values.m_description.alignment = TMPro.TextAlignmentOptions.Center;
            values.m_description.text = m_spellScript.m_data_Aura.SpellDescription;
        }
        // else show upgrades
        else
        {
            values.m_description.text = "Upgrade";
        }
    }
}
