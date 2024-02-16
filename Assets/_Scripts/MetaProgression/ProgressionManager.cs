using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    private void Update()
    {

    }

    public void InitMetaProgression(PlayerController _player)
    {
        m_playerController = _player;
    }

    public void UpdateMetaProgression()
    {
        HealthUpgrade();
        HealthRegenUpgrade();
        DamageUpgrade();
        CritChanceUpgrade();
        CollectionUpgrade();
        MoveSpeedUpgrade();
        GoldMultiUpgrade();
        XPUpgrade();
    }

    private int m_healthLevel;
    [SerializeField] private float[] m_healthStats;

    public void UpgradeHealth()
    {
      
            m_healthLevel++;
            m_healthLevel = 5;
        
        HealthUpgrade();

    }

    private void HealthUpgrade()
    {
        for (int i = 0; i < m_healthLevel; i++)
        {
            m_playerController.ActivePlayerData.MaxHealth += m_healthStats[i];
            Debug.Log("Health upgraded " + i);
            Debug.Log("Health = " + m_playerController.ActivePlayerData.MaxHealth);
        }
    }

    private int m_healthRegenLevel;
    [SerializeField] private float[] m_healthRegenStats;

    public void UpgradeHealthRegen()
    {
        m_healthRegenLevel++;
        HealthRegenUpgrade();
    }

    private void HealthRegenUpgrade()
    {
        for (int i = 0; i < m_healthRegenLevel; i++)
        {
            m_playerController.ActivePlayerData.HealthRegeneration += m_healthRegenStats[i];
        }
    }

    private int m_damageMultiLevel;
    [SerializeField] private float[] m_damageStats;

    public void UpgradeDamage()
    {
        m_damageMultiLevel++;
        DamageUpgrade();
    }

    private void DamageUpgrade()
    {
        for (int i = 0; i < m_damageMultiLevel; i++)
        {
            m_playerController.ActivePlayerData.DamageMultiplier += m_damageStats[i];
        }
    }

    private int m_critChanceLevel;
    [SerializeField] private float[] m_critChanceStats;

    public void UpgradeCritChance()
    {
        m_critChanceLevel++;
        CritChanceUpgrade();
    }

    private void CritChanceUpgrade()
    {
        for (int i = 0; i < m_critChanceLevel; i++)
        {
            m_playerController.ActivePlayerData.CritChance += m_critChanceStats[i];
        }
    }

    private int m_collectionLevel;
    [SerializeField] private float[] m_collectionStats;

    public void UpgradeCollectionRadius()
    {
        m_collectionLevel++;
        CollectionUpgrade();
    }

    private void CollectionUpgrade()
    {
        for (int i = 0; i < m_collectionLevel; i++)
        {
            m_playerController.ActivePlayerData.CollectionRadius += m_collectionStats[i];
        }
    }

    private int m_moveSpeedLevel;
    [SerializeField] private float[] m_moveSpeedStats;

    public void UpgradeMovementSpeed()
    {
        m_moveSpeedLevel++;
        MoveSpeedUpgrade();
    }

    private void MoveSpeedUpgrade()
    {
        for (int i = 0; i < m_moveSpeedLevel; i++)
        {
            m_playerController.ActivePlayerData.MovementSpeed += m_moveSpeedStats[i];
        }
    }

    private int m_goldLevel;
    [SerializeField] private float[] m_goldStats;

    public void UpgradeGoldMulti()
    {
        m_goldLevel++;
        GoldMultiUpgrade();
    }

    private void GoldMultiUpgrade()
    {
        for (int i = 0; i < m_goldLevel; i++)
        {
            m_playerController.ActivePlayerData.GoldMultiplier += m_goldStats[i];
        }
    }

    private int m_xpLevel;
    [SerializeField] private float[] m_xpStats;

    public void UpgradeXP()
    {
        m_xpLevel++;
        XPUpgrade();
    }

    private void XPUpgrade()
    {
        for (int i = 0; i < m_xpLevel; i++)
        {
            m_playerController.ActivePlayerData.XPMultiplier += m_xpStats[i];
        }
    }


    public void LoadData(GameData _data)
    {
        this.m_healthLevel = _data.HealthLevel;
        this.m_healthRegenLevel = _data.HealthRegenLevel;
        this.m_damageMultiLevel = _data.DamageLevel;
        this.m_critChanceLevel = _data.CriticalChanceLevel;
        this.m_collectionLevel = _data.CollectionRadiusLevel;
        this.m_moveSpeedLevel = _data.MovementSpeedLevel;
        this.m_goldLevel = _data.GoldLevel;
        this.m_xpLevel = _data.XPLevel;




    }

    public void SaveData(ref GameData _data)
    {
        _data.HealthLevel = this.m_healthLevel;
        _data.HealthRegenLevel = this.m_healthRegenLevel;
        _data.DamageLevel = this.m_damageMultiLevel;
        _data.CriticalChanceLevel = this.m_critChanceLevel;
        _data.CollectionRadiusLevel = this.m_collectionLevel;
        _data.MovementSpeedLevel = this.m_moveSpeedLevel;
        _data.GoldLevel = this.m_goldLevel;
        _data.XPLevel = this.m_xpLevel;

    }
}
