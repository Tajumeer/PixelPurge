using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour, IDataPersistence
{
    private static ProgressionManager m_instance;

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

    //TODO:: player cash check? SO restructure?

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
        m_defaultMaxHealth = m_playerController.ActivePlayerData.MaxHealth;
        m_defaultAttackSpeed = m_playerController.ActivePlayerData.AttackSpeed;
        m_defaultCriticalDamage = m_playerController.ActivePlayerData.CritMultiplier;
        m_defaultMoveSpeed = m_playerController.ActivePlayerData.MovementSpeed;
        UpdateMetaProgression();
    }

    private void UpdateMetaProgression()
    {
        HealthUpgrade();
        AttackSpeedUpgrade();
    }

    #region Health

    private int m_healthLevel;

    private bool m_isHealthLevelOne;
    private bool m_isHealthLevelTwo;
    private bool m_isHealthLevelThree;
    private bool m_isHealthLevelFour;
    private bool m_isHealthLevelFive;

    private float m_defaultMaxHealth;

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
                    m_playerController.ActivePlayerData.MaxHealth = m_defaultMaxHealth * 1.05f;
                    m_isHealthLevelOne = true;
                }
                break;
            case 2:
                if (!m_isHealthLevelTwo)
                {
                    m_playerController.ActivePlayerData.MaxHealth = m_defaultMaxHealth * 1.1f;
                    m_isHealthLevelTwo = true;
                }
                break;
            case 3:
                if (!m_isHealthLevelThree)
                {
                    m_playerController.ActivePlayerData.MaxHealth = m_defaultMaxHealth * 1.15f;
                    m_isHealthLevelTwo = true;
                }
                break;
            case 4:
                if (!m_isHealthLevelFour)
                {
                    m_playerController.ActivePlayerData.MaxHealth = m_defaultMaxHealth * 1.2f;
                    m_isHealthLevelFour = true;
                }
                break;
            case 5:
                if (!m_isHealthLevelFive)
                {
                    m_playerController.ActivePlayerData.MaxHealth = m_defaultMaxHealth * 1.25f;
                    m_isHealthLevelTwo = true;
                }
                break;
            default:
                break;
        }

        Debug.Log(m_playerController.ActivePlayerData.MaxHealth);
    }

    #endregion

    #region Attack Speed

    private int m_attackSpeedLevel;

    private bool m_isAttackSpeedLevelOne;
    private bool m_isAttackSpeedLevelTwo;
    private bool m_isAttackSpeedLevelThree;
    private bool m_isAttackSpeedLevelFour;
    private bool m_isAttackSpeedLevelFive;

    private float m_defaultAttackSpeed;

    public void UpgradeAttackSpeed()
    {
        m_attackSpeedLevel++;
        AttackSpeedUpgrade();
    }

    private void AttackSpeedUpgrade()
    {
        switch (m_attackSpeedLevel)
        {
            case 0:
                break;
            case 1:
                if (!m_isAttackSpeedLevelOne)
                {

                    m_playerController.ActivePlayerData.AttackSpeed = m_defaultAttackSpeed * .95f;
                    m_isHealthLevelOne = true;
                }
                break;
            case 2:
                if (!m_isAttackSpeedLevelTwo)
                {
                    m_playerController.ActivePlayerData.AttackSpeed = m_defaultMaxHealth * .9f;
                    m_isHealthLevelTwo = true;
                }
                break;
            case 3:
                if (!m_isAttackSpeedLevelThree)
                {
                    m_playerController.ActivePlayerData.AttackSpeed = m_defaultMaxHealth * .85f;
                    m_isHealthLevelThree = true;
                }
                break;
            case 4:
                if (!m_isAttackSpeedLevelFour)
                {
                    m_playerController.ActivePlayerData.AttackSpeed = m_defaultMaxHealth * .8f;
                    m_isHealthLevelFour = true;
                }
                break;
            case 5:
                if (!m_isAttackSpeedLevelFive)
                {
                    m_playerController.ActivePlayerData.AttackSpeed = m_defaultMaxHealth * .75f;
                    m_isAttackSpeedLevelFive = true;
                }
                break;
            default:
                break;
        }

        Debug.Log(m_playerController.ActivePlayerData.AttackSpeed);
    }

    #endregion

    #region CritDamage

    private int m_criticalDamageLevel;

    private bool m_isCriticalDamageLevelOne;
    private bool m_isCriticalDamageLevelTwo;
    private bool m_isCriticalDamageLevelThree;
    private bool m_isCriticalDamageLevelFour;
    private bool m_isCriticalDamageLevelFive;

    private float m_defaultCriticalDamage;

    public void UpgradeCriticalDamage()
    {
        m_criticalDamageLevel++;
        CriticalDamageUpgrade();
    }

    private void CriticalDamageUpgrade()
    {
        switch (m_criticalDamageLevel)
        {
            case 0:
                break;
            case 1:
                if (!m_isCriticalDamageLevelOne)
                {

                    m_playerController.ActivePlayerData.CritMultiplier = m_defaultCriticalDamage + .1f;
                    m_isCriticalDamageLevelOne = true;
                }
                break;
            case 2:
                if (!m_isCriticalDamageLevelTwo)
                {
                    m_playerController.ActivePlayerData.CritMultiplier = m_defaultMaxHealth + .2f;
                    m_isCriticalDamageLevelTwo = true;
                }
                break;
            case 3:
                if (!m_isCriticalDamageLevelThree)
                {
                    m_playerController.ActivePlayerData.CritMultiplier = m_defaultMaxHealth + .3f;
                    m_isCriticalDamageLevelThree = true;
                }
                break;
            case 4:
                if (!m_isCriticalDamageLevelFour)
                {
                    m_playerController.ActivePlayerData.CritMultiplier = m_defaultMaxHealth + .4f;
                    m_isCriticalDamageLevelFour = true;
                }
                break;
            case 5:
                if (!m_isCriticalDamageLevelFive)
                {
                    m_playerController.ActivePlayerData.CritMultiplier = m_defaultMaxHealth + .5f;
                    m_isCriticalDamageLevelFive = true;
                }
                break;
            default:
                break;
        }

        Debug.Log(m_playerController.ActivePlayerData.CritMultiplier);
    }

    #endregion

    #region MoveSpeed

    private int m_moveSpeedLevel;

    private bool m_isMoveSpeedLevelOne;
    private bool m_isMoveSpeedLevelTwo;
    private bool m_isMoveSpeedLevelThree;
    private bool m_isMoveSpeedLevelFour;
    private bool m_isMoveSpeedLevelFive;

    private float m_defaultMoveSpeed;

    public void UpgradeMoveSpeed()
    {
        m_moveSpeedLevel++;
        MoveSpeedUpgrade();
    }

    private void MoveSpeedUpgrade()
    {
        switch (m_moveSpeedLevel)
        {
            case 0:
                break;
            case 1:
                if (!m_isMoveSpeedLevelOne)
                {

                    m_playerController.ActivePlayerData.MovementSpeed = m_defaultMoveSpeed * 1.02f;
                    m_isMoveSpeedLevelOne = true;
                }
                break;
            case 2:
                if (!m_isMoveSpeedLevelTwo)
                {
                    m_playerController.ActivePlayerData.MovementSpeed = m_defaultMoveSpeed * 1.04f;
                    m_isMoveSpeedLevelTwo = true;
                }
                break;
            case 3:
                if (!m_isMoveSpeedLevelThree)
                {
                    m_playerController.ActivePlayerData.MovementSpeed = m_defaultMoveSpeed * 1.06f;
                    m_isMoveSpeedLevelThree = true;
                }
                break;
            case 4:
                if (!m_isMoveSpeedLevelFour)
                {
                    m_playerController.ActivePlayerData.MovementSpeed = m_defaultMoveSpeed * 1.08f;
                    m_isMoveSpeedLevelFour = true;
                }
                break;
            case 5:
                if (!m_isMoveSpeedLevelFive)
                {
                    m_playerController.ActivePlayerData.MovementSpeed = m_defaultMoveSpeed * 1.1f;
                    m_isMoveSpeedLevelFive = true;
                }
                break;
            default:
                break;
        }

        Debug.Log(m_playerController.ActivePlayerData.MovementSpeed);
    }

    #endregion

    #region CollectionRadius

    private int m_collectionRadiusLevel;

    private bool m_isCollectionRadiusLevelOne;
    private bool m_isCollectionRadiusLevelTwo;
    private bool m_isCollectionRadiusLevelThree;
    private bool m_isCollectionRadiusLevelFour;
    private bool m_isCollectionRadiusLevelFive;

    private float m_defaultCollectionRadius;

    public void UpgradeCollectionRadius()
    {
        m_collectionRadiusLevel++;
        CollectionRadiusUpgrade();
    }

    private void CollectionRadiusUpgrade()
    {
        switch (m_collectionRadiusLevel)
        {
            case 0:
                break;
            case 1:
                if (!m_isCollectionRadiusLevelOne)
                {

                    m_playerController.ActivePlayerData.CollectionRadius = m_defaultCollectionRadius * 1.1f;
                    m_isCollectionRadiusLevelOne = true;
                }
                break;
            case 2:
                if (!m_isCollectionRadiusLevelTwo)
                {
                    m_playerController.ActivePlayerData.CollectionRadius = m_defaultCollectionRadius * 1.2f;
                    m_isCollectionRadiusLevelTwo = true;
                }
                break;
            case 3:
                if (!m_isCollectionRadiusLevelThree)
                {
                    m_playerController.ActivePlayerData.CollectionRadius = m_defaultCollectionRadius * 1.06f;
                    m_isCollectionRadiusLevelThree = true;
                }
                break;
            case 4:
                if (!m_isCollectionRadiusLevelFour)
                {
                    m_playerController.ActivePlayerData.CollectionRadius = m_defaultCollectionRadius * 1.08f;
                    m_isCollectionRadiusLevelFour = true;
                }
                break;
            case 5:
                if (!m_isCollectionRadiusLevelFive)
                {
                    m_playerController.ActivePlayerData.CollectionRadius = m_defaultCollectionRadius * 1.1f;
                    m_isCollectionRadiusLevelFive = true;
                }
                break;
            default:
                break;
        }

        Debug.Log(m_playerController.ActivePlayerData.CollectionRadius);
    }

    #endregion

    #region IDataPersistence

    public void LoadData(GameData _data)
    {
        this.m_attackSpeedLevel = _data.AttackSpeedLevel;
        this.m_collectionRadiusLevel = _data.CollectionRadiusLevel;
        this.m_criticalDamageLevel = _data.CriticalDamageLevel;
        this.m_healthLevel = _data.HealthLevel;
        this.m_moveSpeedLevel = _data.MovementSpeedLevel;
    }

    public void SaveData(ref GameData _data)
    {
        //Debug.Log("Data To Save: " + m_healthLevel);
        _data.AttackSpeedLevel = this.m_attackSpeedLevel;
        _data.CollectionRadiusLevel = this.m_collectionRadiusLevel;
        _data.CriticalDamageLevel = this.m_criticalDamageLevel;
        _data.HealthLevel = this.m_healthLevel;
        _data.MovementSpeedLevel = this.m_moveSpeedLevel;
       
    }

    #endregion

}
