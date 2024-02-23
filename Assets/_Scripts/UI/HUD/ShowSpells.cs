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

    [HideInInspector] public List<Spells> LearnedActiveSpells;
    [HideInInspector] public List<Spells> LearnedPassiveSpells;

    private void OnEnable()
    {
        m_cd = new List<float>();
        m_timer = new List<float>();

        LearnedActiveSpells = new List<Spells>();
        LearnedPassiveSpells = new List<Spells>();

        for (int i = 0; i < m_sprites_active.Length; i++)
        {
            m_sprites_active[i].gameObject.SetActive(false);
            m_sprites_passive[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        for (int i = 0; i < LearnedActiveSpells.Count; i++)
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
    public void LearnActiveSpell(Spells _spell)
    {
        SO_ActiveSpells spellSO = m_dataSpells.activeSpellSO[(int)_spell];

        // activate and set sprite icon
        m_sprites_active[LearnedActiveSpells.Count].gameObject.SetActive(true);
        m_sprites_active[LearnedActiveSpells.Count].sprite = spellSO.SpellIcon;

        LearnedActiveSpells.Add(_spell);

        // set cd values
        m_cd.Add(spellSO.Cd[spellSO.Level - 1]);
        m_timer.Add(0f);

    }

    public void LearnPassiveSpell(Spells _spell)
    {
        SO_PassiveSpells spellSO = m_dataSpells.passiveSpellSO[(int)_spell - ((int)Spells.ActiveSpells + 1)];

        // activate and set sprite icon
        m_sprites_passive[LearnedPassiveSpells.Count].gameObject.SetActive(true);
        m_sprites_passive[LearnedPassiveSpells.Count].sprite = spellSO.SpellIcon;

        LearnedPassiveSpells.Add(_spell);
    }
}
