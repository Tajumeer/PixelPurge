using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Maya

public class ShowSpells : MonoBehaviour
{
    [SerializeField] private SO_AllSpellSO m_spellSO;

    [Header("Active Spells")]
    [SerializeField] public Image[] m_sprites_active;
    [SerializeField] private Image[] m_cdImage;

    [Header("Passive Spells")]
    [SerializeField] public Image[] m_sprites_passive;

    /// <summary>
    /// the cd this spell has
    /// </summary>
    private List<float> m_cds;
    /// <summary>
    /// the active CD counter for this spell
    /// </summary>
    private List<float> m_activeCds;

    private List<Spells> m_learnedActiveSpells;
    private List<Spells> m_learnedPassiveSpells;

    private void OnEnable()
    {
        Debug.Log("SO "+m_spellSO.AllDirections.Cd);
        m_cds = new List<float>();
        m_activeCds = new List<float>();
        m_learnedActiveSpells = new List<Spells>();
        m_learnedPassiveSpells = new List<Spells>();
    }

    private void Update()
    {
        for (int i = 0; i < m_cds.Count; i++)
        {
            m_activeCds[i] += Time.deltaTime;                               // cd of the spells
            m_cdImage[i].fillAmount = 1f - (m_activeCds[i] / m_cds[i]);     // set cd image in HUD inverted (1->0)

            if (m_activeCds[i] >= m_cds[i])
            {
                m_activeCds[i] = 0f;
                m_cdImage[i].fillAmount = 0f;
            }
        }
    }

    /// <summary>
    /// Shows an Icon of a new learned spell in the HUD
    /// </summary>
    /// <param name="_icon">The Sprite of the Icon that should bne showed in the HUD</param>
    /// <param name="_active">Is the Spell active (true) or passie (false)</param>
    /// <param name="_cd">The CD of the active spell</param>
    public void ShowNewSpell(Spells _spell, Sprite _icon, bool _active, float _cd = 0f)
    {
        if (_active)
        {
            m_sprites_active[m_learnedActiveSpells.Count].sprite = _icon;
            m_learnedActiveSpells.Add(_spell);

            m_cds.Add(_cd);
            m_activeCds.Add(0f);
        }
        else
        {
            m_sprites_passive[m_learnedPassiveSpells.Count].sprite = _icon;
            m_learnedPassiveSpells.Add(_spell);
        }
    }

    /// <summary>
    /// Changes CD of the Spell Icon to the inserted number
    /// </summary>
    /// <param name="_spell">The CD of which spell should be changes</param>
    /// <param name="_newCD">what is the new CD of this spell</param>
    public void ChangeCDofSpell(Spells _spell, float _newCD)
    {
        for (int i = 0; i < m_learnedActiveSpells.Count; i++)
        {
            if (m_learnedActiveSpells[i] == _spell)
            {
                m_cds[i] = _newCD;
            }
        }
    }
}
