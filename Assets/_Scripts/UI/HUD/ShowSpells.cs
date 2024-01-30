using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Maya

public class ShowSpells : MonoBehaviour
{
    [SerializeField] private SO_AllSpells m_dataSpells;

    [Header("Active Spells")]
    [SerializeField] public Image[] m_sprites_active;
    [SerializeField] private Image[] m_cdImage;

    [Header("Passive Spells")]
    [SerializeField] public Image[] m_sprites_passive;

    /// <summary>
    /// the cd this spell has
    /// </summary>
    private List<float> m_cd;
    /// <summary>
    /// the active CD timer for this spell
    /// </summary>
    private List<float> m_timer;

    private List<Spells> m_learnedActiveSpells;
    private List<Spells> m_learnedPassiveSpells;

    private void OnEnable()
    {
        m_cd = new List<float>();
        m_timer = new List<float>();

        m_learnedActiveSpells = new List<Spells>();
        m_learnedPassiveSpells = new List<Spells>();

        for (int i = 0; i < m_sprites_active.Length; i++)
        {
            m_sprites_active[i].gameObject.SetActive(false);
            m_sprites_passive[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        for (int i = 0; i < m_learnedActiveSpells.Count; i++)
        {
            m_timer[i] += Time.deltaTime;                               // cd of the spells
            m_cdImage[i].fillAmount = 1f - (m_timer[i] / m_cd[i]);      // set cd image in HUD inverted (1->0)

            if (m_timer[i] >= m_cd[i])
            {
                m_timer[i] = 0f;
                m_cdImage[i].fillAmount = 0f;
            }
        }
    }

    /// <summary>
    /// Shows an Icon of a new learned spell in the HUD
    /// </summary>
    public void LearnNewSpell(Spells _spell)
    {
        SO_Spells spellSO = m_dataSpells.spellSO[(int)_spell];

        // check if its active or passive
        if ((int)_spell < (int)Spells.ActiveSpells)
        {
            // activate and set sprite icon
            m_sprites_active[m_learnedActiveSpells.Count].gameObject.SetActive(true);
            m_sprites_active[m_learnedActiveSpells.Count].sprite = spellSO.SpellIcon;

            m_learnedActiveSpells.Add(_spell);

            // set cd values
            m_cd.Add(spellSO.Cd[spellSO.Level - 1]);
            m_timer.Add(0f);
        }
        else if ((int)_spell > (int)Spells.ActiveSpells && (int)_spell < (int)Spells.PassiveSpells)
        {
            // activate and set sprite icon
            m_sprites_passive[m_learnedPassiveSpells.Count].gameObject.SetActive(true);
            m_sprites_passive[m_learnedPassiveSpells.Count].sprite = spellSO.SpellIcon;

            m_learnedPassiveSpells.Add(_spell);
        }
    }
}
