using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// Maya

public enum Spells
{
    AllDirections = 0,
    NearPlayer,
    BaseArcher,
    Aura,
    SpellAmount
}

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
    private bool[] m_active = new bool[(int)Spells.SpellAmount];

    [Header("Spell Timer")]
    private float[] m_timer = new float[(int)Spells.SpellAmount];

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
    private Transform[] m_parent = new Transform[(int)Spells.SpellAmount];
    private Transform m_parent_Spells;

    [Header("Spell Levels")]
    private int[] m_level = new int[(int)Spells.SpellAmount];

    private float[] m_cd = new float[(int)Spells.SpellAmount];

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
        for (int i = 0; i < (int)Spells.SpellAmount; i++)
        {
            if (!m_active[i]) continue;     // skip if spell is not learned

            m_timer[i] += Time.deltaTime;
            if (m_timer[i] >= m_cd[i])      // if spell is ready
            {
                switch (i)                  // check which spell it was and spawn it
                {
                    case (int)Spells.AllDirections:
                        spawnScript.SpawnAllDirections(m_data_AllDirections, m_pool_AllDirections, m_parent[(int)Spells.AllDirections]);
                        break;

                    case (int)Spells.BaseArcher:
                        spawnScript.SpawnBaseArcher(m_data_BaseArcher, m_pool_BaseArcher, m_parent[(int)Spells.BaseArcher]);
                        break;

                    case (int)Spells.NearPlayer:
                        spawnScript.SpawnNearPlayer(m_data_NearPlayer, m_pool_NearPlayer, m_parent[(int)Spells.NearPlayer]);
                        break;

                    case (int)Spells.Aura:

                        break;
                }
                m_timer[i] = 0;             // reset the timer
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
        m_active[(int)Spells.BaseArcher] = true;

        m_cd[(int)Spells.BaseArcher] = m_data_BaseArcher.Cd;

        // Create ObjectPool
        m_pool_BaseArcher = new ObjectPool<Spell_BaseArcher>(m_prefab_BaseArcher);

        // Create Spell Parent GameObject 
        GameObject obj = new GameObject();
        obj.name = "BaseArcher";
        obj.transform.SetParent(m_parent_Spells.transform);
        m_parent[(int)Spells.BaseArcher] = obj.transform;
    }

    /// <summary>
    /// Learn the Spell "AllDirections" and show it in UI
    /// </summary>
    public void LearnAllDirections()
    {
        m_level[(int)Spells.AllDirections] = 1;
        m_data_AllDirections = Instantiate(m_data_AllDirections_original);
        m_active[(int)Spells.AllDirections] = true;

        m_cd[(int)Spells.AllDirections] = m_data_AllDirections.Cd;

        // Create ObjectPool
        m_pool_AllDirections = new ObjectPool<Spell_AllDirections>(m_prefab_AllDirections);

        // Create Spell Parent GameObject 
        GameObject obj = new GameObject();
        obj.name = "AllDirections";
        obj.transform.SetParent(m_parent_Spells.transform);
        m_parent[(int)Spells.AllDirections] = obj.transform;

        // UI
    }

    /// <summary>
    /// Learn the Spell "NearPlayer" and show it in UI
    /// </summary>
    public void LearnNearPlayer()
    {
        m_level[(int)Spells.NearPlayer] = 1;
        m_data_NearPlayer = Instantiate(m_data_NearPlayer_original);
        m_active[(int)Spells.NearPlayer] = true;

        m_cd[(int)Spells.NearPlayer] = m_data_NearPlayer.Cd;

        // Create ObjectPool
        m_pool_NearPlayer = new ObjectPool<Spell_NearPlayer>(m_prefab_NearPlayer);

        // Create Spell Parent GameObject 
        GameObject obj = new GameObject();
        obj.name = "NearPlayer";
        obj.transform.SetParent(m_parent_Spells.transform);
        m_parent[(int)Spells.NearPlayer] = obj.transform;

        // UI
    }

    /// <summary>
    /// Learn the Spell "Aura" and show it in UI
    /// </summary>
    public void LearnAura()
    {
        m_level[(int)Spells.Aura] = 1;
        m_data_Aura = Instantiate(m_data_Aura_original);

        m_cd[(int)Spells.Aura] = m_data_Aura.Cd;

        // Create Spell Parent GameObject 
        GameObject auraObj = Instantiate(m_prefab_Aura, FindObjectOfType<PlayerController>().gameObject.transform);
        auraObj.name = "Aura";
        m_parent[(int)Spells.Aura] = auraObj.transform;

        auraObj.GetComponent<Spell_Aura>().OnSpawn(m_data_Aura);

        // UI
    }

    #endregion

    public void UpgradeSpell(Spells _spell)
    {
        switch (_spell)                  // check which spell it was and upgrade it
        {
            case Spells.AllDirections:
                m_data_AllDirections.Damage *= (1f + m_data_Upgrades.Damage[m_level[(int)_spell]]);
                break;

            case Spells.BaseArcher:
                
                break;

            case Spells.NearPlayer:
                m_data_NearPlayer.Damage *= (1f + m_data_Upgrades.Damage[m_level[(int)_spell]]);
                break;

            case Spells.Aura:
                m_data_Aura.Damage *= (1f + m_data_Upgrades.Damage[m_level[(int)_spell]]);
                break;
        }

        m_level[(int)_spell]++;
    }
}
