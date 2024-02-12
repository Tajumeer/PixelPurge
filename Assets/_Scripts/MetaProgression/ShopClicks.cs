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
    
    
    }

    public void ClickHealthRegenerationUpgrade()
    {
     
    }

    public void ClickDamageUpgrade()
    {
    
    }

    public void ClickCritChanceUpgrade()
    {
       
    }

    public void ClickCollectionRadiusUpgrade()
    {
     
    }

    public void ClickMovementSpeedUpgrade()
    {
     
    }
    
    public void ClickGoldMultiplierUpgrade()
    {
    
    }

    public void ClickXPUpgrade()
    {
    
    }
}
