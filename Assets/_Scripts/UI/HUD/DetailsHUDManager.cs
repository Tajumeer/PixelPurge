using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Maya

public class DetailsHUDManager : MonoBehaviour
{
    private ShowSpells m_spellHUDScript;

    [SerializeField] private SO_AllSpells m_data_Spells;

    [Header("Active Spells")]
    [SerializeField] private Image[] m_sprites_active;
    [SerializeField] private TextMeshProUGUI[] m_level_active;

    [Header("Passive Spells")]
    [SerializeField] private Image[] m_sprites_passive;
    [SerializeField] private TextMeshProUGUI[] m_level_passive;

    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI m_statAmount_maxHealth;
    [SerializeField] private TextMeshProUGUI m_statAmount_healthRegeneration;
    [SerializeField] private TextMeshProUGUI m_statAmount_damageReduction;
    [SerializeField] private TextMeshProUGUI m_statAmount_movementSpeed;
    [SerializeField] private TextMeshProUGUI m_statAmount_collectionRadius;
    [SerializeField] private TextMeshProUGUI m_statAmount_goldMultiplier;
    [SerializeField] private TextMeshProUGUI m_statAmount_damage;
    [SerializeField] private TextMeshProUGUI m_statAmount_critChance;
    [SerializeField] private TextMeshProUGUI m_statAmount_cdReduction;
    [SerializeField] private TextMeshProUGUI m_statAmount_projectileSpeed;

    private void OnEnable()
    {
        m_spellHUDScript = FindObjectOfType<ShowSpells>();
        if (m_spellHUDScript == null)
        {
            Debug.LogError("Could not find ShowSpell Script in DetailsHUD");
            return;
        }

        ShowSpellIconAndLevel();
        ShowStats();
    }

    private void ShowSpellIconAndLevel()
    {
        for (int i = 0; i < 4; i++)
        {
            if(i >= m_spellHUDScript.LearnedActiveSpells.Count)
            {
                m_sprites_active[i].gameObject.SetActive(false);
                m_level_active[i].text = "";
                continue;
            }

            int spellIdx = (int)m_spellHUDScript.LearnedActiveSpells[i];

            m_sprites_active[i].sprite = m_data_Spells.activeSpellSO[spellIdx].SpellIcon;
            m_level_active[i].text = m_data_Spells.activeSpellSO[spellIdx].Level.ToString();
        }

        for (int i = 0; i < 4; i++)
        {
            if (i >= m_spellHUDScript.LearnedPassiveSpells.Count)
            {
                m_sprites_passive[i].gameObject.SetActive(false);
                m_level_passive[i].text = "";
                continue;
            }

            int spellIdx = (int)m_spellHUDScript.LearnedPassiveSpells[i] - ((int)Spells.ActiveSpells + 1);

            m_sprites_passive[i].sprite = m_data_Spells.passiveSpellSO[spellIdx].SpellIcon;
            m_level_passive[i].text = m_data_Spells.passiveSpellSO[spellIdx].Level.ToString();
        }
    }

    private void ShowStats()
    {
        m_statAmount_maxHealth.text = m_data_Spells.statSO.MaxHealth.ToString();
        m_statAmount_healthRegeneration.text = m_data_Spells.statSO.HealthRegeneration.ToString();
        m_statAmount_damageReduction.text = m_data_Spells.statSO.DamageReductionPercentage.ToString();
        m_statAmount_movementSpeed.text = m_data_Spells.statSO.MovementSpeed.ToString();
        m_statAmount_collectionRadius.text = m_data_Spells.statSO.CollectionRadius.ToString();
        m_statAmount_goldMultiplier.text = m_data_Spells.statSO.GoldMultiplier.ToString() + "%";
        m_statAmount_damage.text = m_data_Spells.statSO.DamageMultiplier.ToString() + "%";
        m_statAmount_critChance.text = m_data_Spells.statSO.CritChance.ToString() + "%";
        m_statAmount_projectileSpeed.text = m_data_Spells.statSO.ProjectileSpeed.ToString();
    }
}
