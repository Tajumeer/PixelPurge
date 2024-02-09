using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Maya

public enum UpgradableStats
{
    MaxHealth = 0,
    HealthRegeneration,

    DamageMultiplier,
    CritChance,

    CollectionRadius,
    MovementSpeed,

    GoldMultiplier,
    XPMultiplier,
}

public class ShopManager : MonoBehaviour
{
    [System.Serializable]
    public class GoldAmount
    {
        [Tooltip("The Cost of one stat upgrade at different levels")]
        public int[] m_cost;
    }

    [Tooltip("A List of All Costs of all stats upgrades")]
    [SerializeField] private GoldAmount[] m_cost;
    [Space]
    [Tooltip("The image of the indicator when is has this level")]
    public Sprite m_indicatorImageUpgraded;

    private UiData m_data;

    public void UpgradeStat(ChooseStat _statScript)
    {
        int stat = (int)_statScript.m_stat;

        // set the level indicator image
        _statScript.m_levelIndicator[m_data.StatLevel[stat]].sprite = m_indicatorImageUpgraded;

        // increase level
        m_data.StatLevel[stat]++;

        // if that was the last level of this stat
        if (m_data.StatLevel[stat] == _statScript.m_levelIndicator.Length)
        {
            // disable the button
            _statScript.gameObject.GetComponent<Button>().interactable = false;
            _statScript.m_gold.text = "Max";
        }
        else
        {
            // set gold text to the amount of gold needed for the next level of this stat
            _statScript.m_gold.text = m_cost[stat].m_cost[m_data.StatLevel[stat] - 1].ToString();
        }
    }

    public void InitStat(ChooseStat _statScript)
    {
        if(m_data == null)
            m_data = UiData.Instance;

        int stat = (int)_statScript.m_stat;

        // set the level indicator image
        for (int i = 0; i < m_data.StatLevel[stat]; i++)
        {
            _statScript.m_levelIndicator[i].sprite = m_indicatorImageUpgraded;
        }

        // if that was the last level of this stat
        if (m_data.StatLevel[stat] == _statScript.m_levelIndicator.Length)
        {
            // disable the button
            _statScript.gameObject.GetComponent<Button>().interactable = false;
            _statScript.m_gold.text = "Max";
        }
        else
        {
            // set gold text to the amount of gold needed for the next level of this stat
            _statScript.m_gold.text = m_cost[stat].m_cost[m_data.StatLevel[stat] - 1].ToString();
        }
    }

    public void UnloadShop()
    {
        MenuManager.Instance.UnloadSceneAsync(Scenes.Shop);
    }
}
