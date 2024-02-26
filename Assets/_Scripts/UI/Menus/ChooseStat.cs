using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Maya

public class ChooseStat : MonoBehaviour
{
    public UpgradableStats m_stat;
    public Image[] m_levelIndicator;
    public TextMeshProUGUI m_gold;

    private ShopManager m_manager;

    private void OnEnable()
    {
        // search for ShopManager if null and InitStat()
        if (m_manager == null)
        {
            if (m_manager = FindObjectOfType<ShopManager>())
                m_manager.InitStat(this);
        }
        else m_manager.InitStat(this);
    }

    public void ChooseThisStat()
    {
        // search for ShopManager if null and UpgradeStat()
        if (m_manager == null)
        {
            if (m_manager = FindObjectOfType<ShopManager>())
                m_manager.UpgradeStat(this);
        }
        else m_manager.UpgradeStat(this);
    }
}
