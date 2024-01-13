using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    private static ProgressionManager m_instance;

    private int m_healthLevel;

    private bool m_isHealthLevelOne;
    private bool m_isHealthLevelTwo;
    private bool m_isHealthLevelThree;
    private bool m_isHealthLevelFour;
    private bool m_isHealthLevelFive;

    private float m_defaultMaxHealth;

    public static ProgressionManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                GameObject obj = new GameObject("ProgressionManager");
                m_instance = obj.AddComponent<ProgressionManager>();
            }

            return m_instance;
        }
    }

    private PlayerController m_playerController;
    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitMetaProgression()
    {
        m_playerController = FindObjectOfType<PlayerController>();
        m_defaultMaxHealth = m_playerController.MaxHealth;
        UpdateMetaProgression();
    }

    private void UpdateMetaProgression()
    {
        HealthUpgrade();
    }
    public void UpgradeHealth()
    {
        m_healthLevel++;
        HealthUpgrade();
    }
    private void HealthUpgrade()
    {
        switch (m_healthLevel)
        {
            case 0:
                break;
            case 1:
                if (!m_isHealthLevelOne)
                {
                    m_playerController.MaxHealth = m_defaultMaxHealth * 1.05f;
                    m_isHealthLevelOne = true;
                }
                break;
            case 2:
                if (!m_isHealthLevelTwo)
                {
                    m_playerController.MaxHealth = m_defaultMaxHealth * 1.1f;
                    m_isHealthLevelTwo = true;
                }
                break;
            case 3:
                if (!m_isHealthLevelThree)
                {
                    m_playerController.MaxHealth = m_defaultMaxHealth * 1.15f;
                    m_isHealthLevelTwo = true;
                }
                break;
            case 4:
                if (!m_isHealthLevelFour)
                {
                    m_playerController.MaxHealth = m_defaultMaxHealth * 1.2f;
                    m_isHealthLevelFour = true;
                }
                break;
            case 5:
                if (!m_isHealthLevelFive)
                {
                    m_playerController.MaxHealth = m_defaultMaxHealth * 1.25f;
                    m_isHealthLevelTwo = true;
                }
                break;
            default:
                break;
        }

        Debug.Log(m_playerController.MaxHealth);
    }
}
