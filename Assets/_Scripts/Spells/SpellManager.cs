using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class SpellManager : MonoBehaviour
{
    SpellSpawner spawnScript;

    [Header("Scriptable Objects")]
    [SerializeField] private SO_SpellUpgrades m_data_Upgrades;
    [Space]
    [SerializeField] private SO_AllDirections m_data_AllDirections_original;
    [SerializeField] private SO_NearPlayer m_data_NearPlayer_original;
    [SerializeField] private SO_BaseArcher m_data_BaseArcher_original;
    [SerializeField] private SO_Aura m_data_Aura_original;

    private SO_AllDirections m_data_AllDirections;
    private SO_NearPlayer m_data_NearPlayer;
    private SO_BaseArcher m_data_BaseArcher;
    private SO_Aura m_data_Aura;

    [Header("Active Spells")]
    private bool m_active_AllDirections = false;
    private bool m_active_NearPlayer = false;
    private bool m_active_BaseArcher = false;

    [Header("Spell Timer")]
    private float m_timer_AllDirections = 0f;
    private float m_timer_NearPlayer = 0f;
    private float m_timer_BaseArcher = 0f;

    [Header("Prefabs")]
    [SerializeField] private GameObject m_prefab_AllDirections;
    [SerializeField] private GameObject m_prefab_NearPlayer;
    [SerializeField] private GameObject m_prefab_BaseArcher;
    [SerializeField] private GameObject m_prefab_Aura;

    [Header("Pools")]
    private ObjectPool<Spell_AllDirections> m_pool_AllDirections;
    private ObjectPool<Spell_NearPlayer> m_pool_NearPlayer;
    private ObjectPool<Spell_BaseArcher> m_pool_BaseArcher;

    [Header("Parent Objects")]
    private Transform m_parent_Spells;
    private Transform m_parent_AllDirections;
    private Transform m_parent_NearPlayer;
    private Transform m_parent_BaseArcher;
    private Transform m_parent_Aura;

    [Header("Spell Levels")]
    private int m_level_AllDirections = 1;
    private int m_level_NearPlayer = 1;
    private int m_level_Aura = 1;

    private void OnEnable()
    {
        spawnScript = FindObjectOfType<SpellSpawner>();

        // Create Spell Parent GameObject
        GameObject obj = new GameObject();
        obj.name = "Spells";
        m_parent_Spells = obj.transform;

        // Prototype
        LearnBaseArcher();
        LearnAllDirections();
        LearnNearPlayer();
        LearnAura();
    }

    private void Update()
    {
        // ALL DIRECTIONS
        if (m_active_AllDirections)
        {
            m_timer_AllDirections += Time.deltaTime;
            if (m_timer_AllDirections >= m_data_AllDirections.Cd)
            {
                spawnScript.SpawnAllDirections(m_data_AllDirections, m_pool_AllDirections, m_parent_AllDirections);
                m_timer_AllDirections = 0;
            }
        }

        // NEAR PLAYER
        if (m_active_NearPlayer)
        {
            m_timer_NearPlayer += Time.deltaTime;
            if (m_timer_NearPlayer >= m_data_NearPlayer.Cd)
            {
                spawnScript.SpawnNearPlayer(m_data_NearPlayer, m_pool_NearPlayer, m_parent_NearPlayer);
                m_timer_NearPlayer = 0;
            }
        }

        // BASE ARCHER
        if (m_active_BaseArcher)
        {
            m_timer_BaseArcher += Time.deltaTime;
            if (m_timer_BaseArcher >= m_data_BaseArcher.Cd)
            {
                spawnScript.SpawnBaseArcher(m_data_BaseArcher, m_pool_BaseArcher, m_parent_BaseArcher);
                m_timer_BaseArcher = 0;
            }
        }
    }

    #region Learn New Spells

    /// <summary>
    /// Learn the Spell "AllDirections" and show it in UI
    /// </summary>
    public void LearnBaseArcher()
    {
        m_data_BaseArcher = Instantiate(m_data_BaseArcher_original);
        m_active_BaseArcher = true;

        // Create ObjectPool
        m_pool_BaseArcher = new ObjectPool<Spell_BaseArcher>(m_prefab_BaseArcher);

        // Create Spell Parent GameObject 
        GameObject obj = new GameObject();
        obj.name = "BaseArcher";
        obj.transform.SetParent(m_parent_Spells.transform);
        m_parent_BaseArcher = obj.transform;
    }

    /// <summary>
    /// Learn the Spell "AllDirections" and show it in UI
    /// </summary>
    public void LearnAllDirections()
    {
        m_level_AllDirections = 1;
        m_data_AllDirections = Instantiate(m_data_AllDirections_original);
        m_active_AllDirections = true;

        // Create ObjectPool
        m_pool_AllDirections = new ObjectPool<Spell_AllDirections>(m_prefab_AllDirections);

        // Create Spell Parent GameObject 
        GameObject obj = new GameObject();
        obj.name = "AllDirections";
        obj.transform.SetParent(m_parent_Spells.transform);
        m_parent_AllDirections = obj.transform;

        // UI
    }

    /// <summary>
    /// Learn the Spell "NearPlayer" and show it in UI
    /// </summary>
    public void LearnNearPlayer()
    {
        m_level_NearPlayer = 1;
        m_data_NearPlayer = Instantiate(m_data_NearPlayer_original);
        m_active_NearPlayer = true;

        // Create ObjectPool
        m_pool_NearPlayer = new ObjectPool<Spell_NearPlayer>(m_prefab_NearPlayer);

        // Create Spell Parent GameObject 
        GameObject obj = new GameObject();
        obj.name = "NearPlayer";
        obj.transform.SetParent(m_parent_Spells.transform);
        m_parent_NearPlayer = obj.transform;

        // UI
    }

    /// <summary>
    /// Learn the Spell "Aura" and show it in UI
    /// </summary>
    public void LearnAura()
    {
        m_level_Aura = 1;
        m_data_Aura = Instantiate(m_data_Aura_original);

        // Create Spell Parent GameObject 
        GameObject auraObj = Instantiate(m_prefab_Aura, FindObjectOfType<PlayerController>().gameObject.transform);
        auraObj.name = "Aura";
        m_parent_Aura = auraObj.transform;

        auraObj.GetComponent<Spell_Aura>().OnSpawn(m_data_Aura);

        // UI
    }

    #endregion

    #region Upgrade Spells

    /// <summary>
    /// Upgrade values of the spell and update UI
    /// </summary>
    public void UpradeAllDirections()
    {
        m_data_AllDirections.Damage *= (1f + m_data_Upgrades.Damage[m_level_AllDirections]);

        m_level_AllDirections++;

        // UI
    }

    /// <summary>
    /// Upgrade values of the spell and update UI
    /// </summary>
    public void UpgradeNearPlayer()
    {
        m_data_NearPlayer.Damage *= (1f + m_data_Upgrades.Damage[m_level_NearPlayer]);

        m_level_NearPlayer++;

        // UI
    }

    /// <summary>
    /// Upgrade values of the spell and update UI
    /// </summary>
    public void UpgradeAura()
    {
        m_data_Aura.Damage *= (1f + m_data_Upgrades.Damage[m_level_Aura]);
        

        m_level_Aura++;

        // UI
    }

    #endregion
}
