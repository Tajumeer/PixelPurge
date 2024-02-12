using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShopClicks : MonoBehaviour
{
    private ProgressionManager m_progressionManager;
    private GameManager m_gameManager;
    private AudioClip m_exitClip;
    private AudioClip m_boughtClip;
    private AudioClip m_noGoldClip;

    private void OnEnable()
    {
        m_progressionManager = FindObjectOfType<ProgressionManager>();
        m_gameManager = FindObjectOfType<GameManager>();
    }

    public void ClickHealthUpgrade()
    {
    
        m_progressionManager.UpgradeHealth();
    }

    public void ClickHealthRegenerationUpgrade()
    {
        m_progressionManager.UpgradeHealthRegen();
    }

    public void ClickDamageUpgrade()
    {
        m_progressionManager.UpgradeDamage();
    }

    public void ClickCritChanceUpgrade()
    {
        m_progressionManager.UpgradeCritChance();
    }

    public void ClickCollectionRadiusUpgrade()
    {
        m_progressionManager.UpgradeCollectionRadius();
    }

    public void ClickMovementSpeedUpgrade()
    {
        m_progressionManager.UpgradeMovementSpeed();
    }
    
    public void ClickGoldMultiplierUpgrade()
    {
        m_progressionManager.UpgradeGoldMulti();
    }

    public void ClickXPUpgrade()
    {
        m_progressionManager.UpgradeXP();
    }
}
