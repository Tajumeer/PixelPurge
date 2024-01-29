using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class LevelUpManager : MonoBehaviour
{
    private SpellManager m_spellScript;
    [SerializeField] private SO_AllSpells m_spellSO;

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
            if (i == (int)Spells.BaseArcher && m_spellSO.BaseArcher.Level >= m_spellScript.m_maxSpellLevel) return;
            if (i == (int)Spells.AllDirections && m_spellSO.AllDirections.Level >= m_spellScript.m_maxSpellLevel) return;
            if (i == (int)Spells.NearPlayer && m_spellSO.NearPlayer.Level >= m_spellScript.m_maxSpellLevel) return;
            if (i == (int)Spells.Aura && m_spellSO.Aura.Level >= m_spellScript.m_maxSpellLevel) return;

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
        values.m_icon.sprite = m_spellSO.BaseArcher.SpellIcon;
        values.m_name.text = m_spellSO.BaseArcher.SpellName;

        // if its not already learned, show spell description
        if (m_spellSO.BaseArcher.Level == 0)
        {
            values.m_description.alignment = TMPro.TextAlignmentOptions.Center;
            values.m_description.text = m_spellSO.BaseArcher.SpellDescription;
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
        values.m_icon.sprite = m_spellSO.AllDirections.SpellIcon;
        values.m_name.text = m_spellSO.AllDirections.SpellName;

        // if its not already learned, show spell description
        if (m_spellSO.AllDirections.Level == 0)
        {
            values.m_description.alignment = TMPro.TextAlignmentOptions.Center;
            values.m_description.text = m_spellSO.AllDirections.SpellDescription;
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
        values.m_icon.sprite = m_spellSO.NearPlayer.SpellIcon;
        values.m_name.text = m_spellSO.NearPlayer.SpellName;

        // if its not already learned, show spell description
        if (m_spellSO.NearPlayer.Level == 0)
        {
            values.m_description.alignment = TMPro.TextAlignmentOptions.Center;
            values.m_description.text = m_spellSO.NearPlayer.SpellDescription;
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
        values.m_icon.sprite = m_spellSO.Aura.SpellIcon;
        values.m_name.text = m_spellSO.Aura.SpellName;

        // if its not already learned, show spell description
        if (m_spellSO.Aura.Level == 0)
        {
            values.m_description.alignment = TMPro.TextAlignmentOptions.Center;
            values.m_description.text = m_spellSO.Aura.SpellDescription;
        }
        // else show upgrades
        else
        {
            values.m_description.text = "Upgrade";
        }
    }
}
