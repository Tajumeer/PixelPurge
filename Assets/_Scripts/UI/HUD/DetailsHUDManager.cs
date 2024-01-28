using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Maya

public class DetailsHUDManager : MonoBehaviour
{
    private SpellManager m_spellScript;

    [Header("Active Spells")]
    [SerializeField] private Image[] m_sprites_active;
    [SerializeField] private TextMeshProUGUI[] m_level_active;

    [Header("Passive Spells")]
    [SerializeField] private Image[] m_sprites_passive;
    [SerializeField] private TextMeshProUGUI[] m_level_passive;

    private void Awake()
    {
        m_spellScript = FindObjectOfType<SpellManager>();
    }

    private void OnEnable()
    {
        
    }
}
