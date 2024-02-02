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
        if (m_manager == null)
        {
            if (m_manager = FindObjectOfType<ShopManager>())
                m_manager.InitStat(this);
            else Debug.LogWarning(m_manager.GetType() + " not found");
        }
        else m_manager.InitStat(this);
    }

    public void ChooseThisStat()
    {
        if (m_manager == null)
        {
            if (m_manager = FindObjectOfType<ShopManager>())
                m_manager.UpgradeStat(this);
            else Debug.LogWarning(m_manager.GetType() + " not found");
        }
        else m_manager.UpgradeStat(this);
    }
}
