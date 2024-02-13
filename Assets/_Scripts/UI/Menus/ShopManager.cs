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

    [Space]

    [Header("Audio")]
    [SerializeField] private AudioClip m_exitClip;
    [SerializeField] private AudioClip m_boughtClip;
    [SerializeField] private AudioClip m_noGoldClip;

    private AudioManager m_audioManager;
    private ProgressionManager m_progressionManager;
    private GameManager m_gameManager;
    private UiData m_data;

    private void OnEnable()
    {
        m_progressionManager = FindObjectOfType<ProgressionManager>();
        m_gameManager = FindObjectOfType<GameManager>();
        m_audioManager = FindObjectOfType<AudioManager>();
    }

    /// <summary>
    /// Increase the Level of the Stat by 1 and change the UI (Level Indicator increase and Needed Gold to next level)
    /// (Gets called when the Player clicks on a Stat to upgrade it)
    /// </summary>
    /// <param name="_statScript"></param>
    public void UpgradeStat(ChooseStat _statScript)
    {
        int stat = (int)_statScript.m_stat;

        // HIER CHECK OB PLAYER GENUG GOLD HAT
        // m_cost[stat].m_cost[m_data.StatLevel[stat] - 1] ist das Gold was benötigt wird zum Kaufen
        if(!GoldCheck(stat, m_cost[stat].m_cost[m_data.StatLevel[stat]]))
        {
            return;
        }

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
            _statScript.m_gold.text = m_cost[stat].m_cost[m_data.StatLevel[stat]].ToString();
        }
    }

    public void InitStat(ChooseStat _statScript)
    {
        if (m_data == null)
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
            _statScript.m_gold.text = m_cost[stat].m_cost[m_data.StatLevel[stat]].ToString();
        }
    }

    public void UnloadShop()
    {
        MenuManager.Instance.UnloadSceneAsync(Scenes.Shop);
        Time.timeScale = 1f;
    }

    private bool GoldCheck(int _stat, int _cost)
    {
        switch (_stat)
        {
            case (int)UpgradableStats.MaxHealth:
                if (m_gameManager.Gold >= _cost)
                {
                    m_audioManager.PlaySound(m_boughtClip);
                    m_progressionManager.UpgradeHealth();
                    return true;
                }
                else
                {
                    m_audioManager.PlaySound(m_noGoldClip);
                    return false;
                }
            case (int)UpgradableStats.HealthRegeneration:
                if (m_gameManager.Gold >= _cost)
                {
                    m_audioManager.PlaySound(m_boughtClip);
                    m_progressionManager.UpgradeHealthRegen();
                    return true;
                }
                else
                {
                    m_audioManager.PlaySound(m_noGoldClip);
                    return false;
                }
            case (int)UpgradableStats.DamageMultiplier:
                if (m_gameManager.Gold >= _cost)
                {
                    m_audioManager.PlaySound(m_boughtClip);
                    m_progressionManager.UpgradeDamage();
                    return true;
                }
                else
                {
                    m_audioManager.PlaySound(m_noGoldClip);
                    return false;
                }
            case (int)UpgradableStats.CritChance:
                if (m_gameManager.Gold >= _cost)
                {
                    m_audioManager.PlaySound(m_boughtClip);
                    m_progressionManager.UpgradeCritChance();
                    return true;
                }
                else
                {
                    m_audioManager.PlaySound(m_noGoldClip);
                    return false;
                }
            case (int)UpgradableStats.CollectionRadius:
                if (m_gameManager.Gold >= _cost)
                {
                    m_audioManager.PlaySound(m_boughtClip);
                    m_progressionManager.UpgradeCollectionRadius();
                    return true;
                }
                else
                {
                    m_audioManager.PlaySound(m_noGoldClip);
                    return false;
                }
            case (int)UpgradableStats.MovementSpeed:
                if (m_gameManager.Gold >= _cost)
                {
                    m_audioManager.PlaySound(m_boughtClip);
                    m_progressionManager.UpgradeMovementSpeed();
                    return true;
                }
                else
                {
                    m_audioManager.PlaySound(m_noGoldClip);
                    return false;
                }
            case (int)UpgradableStats.GoldMultiplier:
                if (m_gameManager.Gold >= _cost)
                {
                    m_audioManager.PlaySound(m_boughtClip);
                    m_progressionManager.UpgradeGoldMulti();
                    return true;
                }
                else
                {
                    m_audioManager.PlaySound(m_noGoldClip);
                    return false;
                }
            case (int)UpgradableStats.XPMultiplier:
                if (m_gameManager.Gold >= _cost)
                {
                    m_audioManager.PlaySound(m_boughtClip);
                    m_progressionManager.UpgradeXP();
                    return true;
                }
                else
                {
                    m_audioManager.PlaySound(m_noGoldClip);
                    return false;
                }
            default:
                return false;

        }
    }
}
