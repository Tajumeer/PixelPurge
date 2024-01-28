using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// Maya

[Serializable]
public enum Spells
{
    AllDirections = 0,
    NearPlayer,
    BaseArcher,
    Aura,
    SpellAmount
}
// where to edit Spells: SpellManager, LevelUpManager

public class SpellManager : MonoBehaviour
{
    SpellSpawner spawnScript;
    ShowSpells spellUIScript;

    public int m_maxSpellLevel = 5;

    [Header("Scriptable Objects")]
    [SerializeField] private SO_SpellUpgrades m_data_Upgrades;
    [SerializeField] private SO_AllSpellSO m_data_Spells;
    [Space]
    [SerializeField] private SO_BaseArcher m_data_BaseArcher_original;
    [SerializeField] private SO_AllDirections m_data_AllDirections_original;
    [SerializeField] private SO_NearPlayer m_data_NearPlayer_original;
    [SerializeField] private SO_Aura m_data_Aura_original;

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

    private bool[] m_active = new bool[(int)Spells.SpellAmount];

    private float[] m_timer = new float[(int)Spells.SpellAmount];

    private float[] m_cd = new float[(int)Spells.SpellAmount];

    private void OnEnable()
    {
        spawnScript = FindObjectOfType<SpellSpawner>();

        // Create Spell Parent GameObject
        GameObject obj = new GameObject();
        obj.name = "Spells";
        m_parent_Spells = obj.transform;

        // set start values
        for (int i = 0; i < (int)Spells.SpellAmount; i++)
        {
            m_active[i] = false;
            m_timer[i] = 0f;
        }

        m_data_Spells.BaseArcher = Instantiate(m_data_BaseArcher_original);
        m_data_Spells.AllDirections = Instantiate(m_data_AllDirections_original);
        m_data_Spells.NearPlayer = Instantiate(m_data_NearPlayer_original);
        m_data_Spells.Aura = Instantiate(m_data_Aura_original);

        // Prototype
        LearnBaseArcher();
        //LearnAllDirections();
        //LearnNearPlayer();
        //LearnAura();
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
                        spawnScript.SpawnAllDirections(m_data_Spells.AllDirections, m_pool_AllDirections, m_parent[(int)Spells.AllDirections]);
                        break;

                    case (int)Spells.BaseArcher:
                        spawnScript.SpawnBaseArcher(m_data_Spells.BaseArcher, m_pool_BaseArcher, m_parent[(int)Spells.BaseArcher]);
                        break;

                    case (int)Spells.NearPlayer:
                        spawnScript.SpawnNearPlayer(m_data_Spells.NearPlayer, m_pool_NearPlayer, m_parent[(int)Spells.NearPlayer]);
                        break;

                    case (int)Spells.Aura:

                        break;
                }
                m_timer[i] = 0;             // reset the timer
            }
        }
    }

    public void ChooseNewSpell(Spells _spell)
    {
        spellUIScript = FindObjectOfType<ShowSpells>();

        switch (_spell)
        {
            case Spells.AllDirections:
                if (m_data_Spells.AllDirections.Level != 0)
                    UpgradeSpell(_spell);
                else
                {
                    LearnAllDirections();
                    spellUIScript.ShowNewSpell(Spells.AllDirections);
                }
                break;

            case Spells.Aura:
                if (m_data_Spells.Aura.Level != 0)
                    UpgradeSpell(_spell);
                else
                {
                    LearnAura();
                    spellUIScript.ShowNewSpell(Spells.Aura);
                }
                break;

            case Spells.BaseArcher:
                if (m_data_Spells.BaseArcher.Level != 0)
                    UpgradeSpell(_spell);
                else
                {
                    LearnBaseArcher();
                    spellUIScript.ShowNewSpell(Spells.BaseArcher);
                }
                break;

            case Spells.NearPlayer:
                if (m_data_Spells.NearPlayer.Level != 0)
                    UpgradeSpell(_spell);
                else
                {
                    LearnNearPlayer();
                    spellUIScript.ShowNewSpell(Spells.NearPlayer);
                }
                break;

            default:
                return;
        }

    }

    #region Learn New Spells

    /// <summary>
    /// Learn the Spell "AllDirections" and show it in UI
    /// </summary>
    private void LearnBaseArcher()
    {
        m_data_Spells.BaseArcher.Level = 1;
        m_active[(int)Spells.BaseArcher] = true;

        m_cd[(int)Spells.BaseArcher] = m_data_Spells.BaseArcher.Cd;

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
    private void LearnAllDirections()
    {
        m_data_Spells.AllDirections.Level = 1;
        m_active[(int)Spells.AllDirections] = true;

        m_cd[(int)Spells.AllDirections] = m_data_Spells.AllDirections.Cd;

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
    private void LearnNearPlayer()
    {
        m_data_Spells.NearPlayer.Level = 1;
        m_active[(int)Spells.NearPlayer] = true;

        m_cd[(int)Spells.NearPlayer] = m_data_Spells.NearPlayer.Cd;

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
    private void LearnAura()
    {
        m_data_Spells.Aura.Level = 1;

        m_cd[(int)Spells.Aura] = m_data_Spells.Aura.Cd;

        // Create Spell Parent GameObject 
        GameObject auraObj = Instantiate(m_prefab_Aura, FindObjectOfType<PlayerController>().gameObject.transform);
        auraObj.name = "Aura";
        m_parent[(int)Spells.Aura] = auraObj.transform;

        auraObj.transform.localScale = new Vector3(m_data_Spells.Aura.Radius, m_data_Spells.Aura.Radius, m_data_Spells.Aura.Radius);

        auraObj.GetComponent<Spell_Aura>().OnSpawn(m_data_Spells.Aura);

        // UI
    }

    #endregion

    private void UpgradeSpell(Spells _spell)
    {
        switch (_spell)                  // check which spell it was and upgrade it
        {
            case Spells.AllDirections:
                //m_data_Spells.AllDirections.Damage *= (1f + m_data_Upgrades.Damage[level]);
                spellUIScript.ChangeCDofSpell(_spell, m_data_Spells.AllDirections.Cd);
                m_data_Spells.AllDirections.Level++;
                break;

            case Spells.BaseArcher:

                spellUIScript.ChangeCDofSpell(_spell, m_data_Spells.BaseArcher.Cd);
                m_data_Spells.BaseArcher.Level++;
                break;

            case Spells.NearPlayer:
                //m_data_Spells.NearPlayer.Damage *= (1f + m_data_Upgrades.Damage[level]);
                spellUIScript.ChangeCDofSpell(_spell, m_data_Spells.NearPlayer.Cd);
                m_data_Spells.NearPlayer.Level++;
                break;

            case Spells.Aura:
                //m_data_Spells.Aura.Damage *= (1f + m_data_Upgrades.Damage[level]);
                spellUIScript.ChangeCDofSpell(_spell, m_data_Spells.Aura.Cd);
                m_data_Spells.Aura.Level++;
                break;
        }
    }
}
