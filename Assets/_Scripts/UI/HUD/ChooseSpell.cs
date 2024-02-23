using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Maya

public class ChooseSpell : MonoBehaviour
{
    public Spells m_spell;
    public GameObject m_newText;
    public Image m_icon;
    public TextMeshProUGUI m_name;
    public TextMeshProUGUI m_description;

    [SerializeField] private int m_goldAmount;

    public void ChooseThisSpell()
    {
        FindObjectOfType<SpellManager>().ChooseNewSpell(m_spell);
        FindObjectOfType<DungeonHUDManager>().UnloadLevelUp();
    }

    public void ChooseGold()
    {
        FindObjectOfType<DungeonHUDManager>().UnloadLevelUp();
        GameManager.Instance.AddGold(m_goldAmount);
    }
}
