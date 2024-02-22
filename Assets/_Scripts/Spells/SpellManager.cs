using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

// Maya

/// <summary>
/// First Active Spells then Passive Spells, at last "SpellAmount".
/// Where to edit them manually: SpellManager
/// </summary>
[Serializable]
public enum Spells
{
    BaseArcher = 0,
    Aura,
    Shockwave,
    Bomb,
    PoisonArea,
    GroundMine,
    ProtectiveOrbs,
    ArrowVolley,
    AllDirections,
    NearPlayer,
    ChainLightning,
    Boomerang,
    Shield,
    /// <summary> Indicator: End of Active Spells </summary>
    ActiveSpells,

    MovementSpeed,
    DamageMultiplier,
    CritChance,
    CritMultiplier,
    AreaMultiplier,
    CdReduction,
    MaxHealth,
    HealthRegeneration,
    DamageReductionPercentage,
    CollectionRadius,
    XPMultiplier,
    /// <summary> Indicator: End of Passive Spells </summary>
    PassiveSpells,

    /// <summary> Indicator: End of All Spells </summary>
    SpellAmount
}

public class SpellManager : MonoBehaviour
{
    SpellSpawner m_spawnScript;
    ShowSpells m_spellUIScript;

    [Header("Scriptable Objects")]
    [SerializeField] private SO_AllSpells m_data_Spells;
    [SerializeField] private SO_OriginalSpells m_data_OriginalSpells;

    [Header("Prefabs")]
    [SerializeField] private GameObject m_prefab_AllDirections;
    [SerializeField] private GameObject m_prefab_NearPlayer;
    [SerializeField] private GameObject m_prefab_BaseArcher;
    [SerializeField] private GameObject m_prefab_Aura;
    [SerializeField] private GameObject m_prefab_Boomerang;
    [SerializeField] private GameObject m_prefab_ProtectiveOrbs;
    [SerializeField] private GameObject m_prefab_GroundMine;
    [SerializeField] private GameObject m_prefab_Shockwave;
    [SerializeField] private GameObject m_prefab_Bomb;
    [SerializeField] private GameObject m_prefab_PoisonArea;
    [SerializeField] private GameObject m_prefab_ChainLightning;
    [SerializeField] private GameObject m_prefab_ArrowVolley;
    [SerializeField] private GameObject m_prefab_Shield;

    [Header("Pools")]
    private ObjectPool<Spell_AllDirections> m_pool_AllDirections;
    private ObjectPool<Spell_NearPlayer> m_pool_NearPlayer;
    private ObjectPool<Spell_BaseArcher> m_pool_BaseArcher;
    private ObjectPool<Spell_Boomerang> m_pool_Boomerang;
    private ObjectPool<Spell_ProtectiveOrbs> m_pool_ProtectiveOrbs;
    private ObjectPool<Spell_GroundMine> m_pool_GroundMine;
    private ObjectPool<Spell_Shockwave> m_pool_Shockwave;
    private ObjectPool<Spell_Bomb> m_pool_Bomb;
    private ObjectPool<Spell_PoisonArea> m_pool_PoisonArea;
    private ObjectPool<Spell_ChainLightning> m_pool_ChainLightning;
    private ObjectPool<Spell_ArrowVolley> m_pool_ArrowVolley;
    private ObjectPool<Spell_Shield> m_pool_Shield;

    [Header("Parent Objects")]
    private Transform[] m_parent = new Transform[(int)Spells.ActiveSpells];
    private Transform m_parent_Spells;

    private bool[] m_active = new bool[(int)Spells.ActiveSpells];

    private float[] m_timer = new float[(int)Spells.ActiveSpells];

    //private float[] m_cd = new float[(int)Spells.ActiveSpells];

    private void OnEnable()
    {
        m_spawnScript = FindObjectOfType<SpellSpawner>();

        // Create Spell Parent GameObject
        GameObject obj = new GameObject();
        obj.name = "Spells";
        m_parent_Spells = obj.transform;

        // set start values
        for (int i = 0; i < (int)Spells.ActiveSpells; i++)
        {
            m_active[i] = false;
            m_timer[i] = 0f;
        }

        InitScriptableObject();

        // Learn Base Spell
        //LearnActiveSpell(Spells.ArrowVolley);
        //LearnActiveSpell(Spells.ProtectiveOrbs);
        //LearnActiveSpell(Spells.AllDirections);
        LearnActiveSpell(Spells.BaseArcher);
        //LearnActiveSpell(Spells.Shield);
        LearnActiveSpell(Spells.Aura);
        //LearnActiveSpell(Spells.Bomb);
        //LearnActiveSpell(Spells.PoisonArea);
        //LearnActiveSpell(Spells.Shockwave);
    }

    /// <summary>
    /// Instantiate new Scriptable Objects for this run (passive and active Spells).
    /// This is important, so that the enum order is the same as the scriptable object array order!
    /// </summary>
    private void InitScriptableObject()
    {
        m_data_Spells.activeSpellSO = new SO_ActiveSpells[(int)Spells.ActiveSpells];

        // Init Active Spells
        for (int i = 0; i < (int)Spells.ActiveSpells; i++)
        {
            switch ((Spells)i)
            {
                case Spells.BaseArcher:
                    m_data_Spells.activeSpellSO[i] = Instantiate(m_data_OriginalSpells.Data_BaseArcher);
                    break;

                case Spells.AllDirections:
                    m_data_Spells.activeSpellSO[i] = Instantiate(m_data_OriginalSpells.Data_AllDirections);
                    break;

                case Spells.NearPlayer:
                    m_data_Spells.activeSpellSO[i] = Instantiate(m_data_OriginalSpells.Data_NearPlayer);
                    break;

                case Spells.Aura:
                    m_data_Spells.activeSpellSO[i] = Instantiate(m_data_OriginalSpells.Data_Aura);
                    break;

                case Spells.Boomerang:
                    m_data_Spells.activeSpellSO[i] = Instantiate(m_data_OriginalSpells.Data_Boomerang);
                    break;

                case Spells.ProtectiveOrbs:
                    m_data_Spells.activeSpellSO[i] = Instantiate(m_data_OriginalSpells.Data_ProtectiveOrbs);
                    break;

                case Spells.GroundMine:
                    m_data_Spells.activeSpellSO[i] = Instantiate(m_data_OriginalSpells.Data_GroundMine);
                    break;
                    
                case Spells.Shockwave:
                    m_data_Spells.activeSpellSO[i] = Instantiate(m_data_OriginalSpells.Data_Shockwave);
                    break;
                    
                case Spells.Bomb:
                    m_data_Spells.activeSpellSO[i] = Instantiate(m_data_OriginalSpells.Data_Bomb);
                    break;
                    
                case Spells.PoisonArea:
                    m_data_Spells.activeSpellSO[i] = Instantiate(m_data_OriginalSpells.Data_PoisonArea);
                    break;
                    
                case Spells.ChainLightning:
                    m_data_Spells.activeSpellSO[i] = Instantiate(m_data_OriginalSpells.Data_ChainLightning);
                    break;
                    
                case Spells.ArrowVolley:
                    m_data_Spells.activeSpellSO[i] = Instantiate(m_data_OriginalSpells.Data_ArrowVolley);
                    break;
                    
                case Spells.Shield:
                    m_data_Spells.activeSpellSO[i] = Instantiate(m_data_OriginalSpells.Data_Shield);
                    break;
            }
        }

        m_data_Spells.passiveSpellSO = new SO_PassiveSpells[(int)Spells.PassiveSpells - (int)Spells.ActiveSpells];

        // Init Passive Spells
        for (int i = (int)Spells.ActiveSpells + 1; i < (int)Spells.PassiveSpells; i++)
        {
            // get the index of the passive spell ("delete" active Spells for index)
            int idx = i - ((int)Spells.ActiveSpells + 1);

            switch ((Spells)i)
            {
                case Spells.MovementSpeed:
                    m_data_Spells.passiveSpellSO[idx] = Instantiate(m_data_OriginalSpells.Data_MovementSpeed);
                    break;

                case Spells.DamageMultiplier:
                    m_data_Spells.passiveSpellSO[idx] = Instantiate(m_data_OriginalSpells.Data_DamageMultiplier);
                    break;

                case Spells.CritChance:
                    m_data_Spells.passiveSpellSO[idx] = Instantiate(m_data_OriginalSpells.Data_CritChance);
                    break;

                case Spells.CritMultiplier:
                    m_data_Spells.passiveSpellSO[idx] = Instantiate(m_data_OriginalSpells.Data_CritMultiplier);
                    break;

                case Spells.AreaMultiplier:
                    m_data_Spells.passiveSpellSO[idx] = Instantiate(m_data_OriginalSpells.Data_AreaMultiplier);
                    break;

                case Spells.CdReduction:
                    m_data_Spells.passiveSpellSO[idx] = Instantiate(m_data_OriginalSpells.Data_CdReduction);
                    break;

                case Spells.MaxHealth:
                    m_data_Spells.passiveSpellSO[idx] = Instantiate(m_data_OriginalSpells.Data_MaxHealth);
                    break;

                case Spells.HealthRegeneration:
                    m_data_Spells.passiveSpellSO[idx] = Instantiate(m_data_OriginalSpells.Data_HealthRegeneration);
                    break;

                case Spells.DamageReductionPercentage:
                    m_data_Spells.passiveSpellSO[idx] = Instantiate(m_data_OriginalSpells.Data_DamageReductionPercentage);
                    break;

                case Spells.CollectionRadius:
                    m_data_Spells.passiveSpellSO[idx] = Instantiate(m_data_OriginalSpells.Data_CollectionRadius);
                    break;

                case Spells.XPMultiplier:
                    m_data_Spells.passiveSpellSO[idx] = Instantiate(m_data_OriginalSpells.Data_XPMultiplier);
                    break;
            }
        }

    }

    private void Update()
    {
        for (int i = 0; i < (int)Spells.ActiveSpells; i++)
        {
            SO_ActiveSpells spellSO = m_data_Spells.activeSpellSO[i];

            if (spellSO == null) continue;   // safety check if its a spell
            if (!m_active[i]) continue;     // skip if spell is not learned

            m_timer[i] += Time.deltaTime;
            if (m_timer[i] >= spellSO.Cd[spellSO.Level - 1] * m_data_Spells.statSO.CdReduction)      // if spell is ready
            {
                switch (i)                  // check which spell it was and spawn it
                {
                    case (int)Spells.AllDirections:
                        m_spawnScript.SpawnAllDirections(m_data_Spells.statSO, m_data_Spells.activeSpellSO[(int)Spells.AllDirections], m_pool_AllDirections, m_parent[(int)Spells.AllDirections]);
                        break;

                    case (int)Spells.BaseArcher:
                        m_spawnScript.SpawnBaseArcher(m_data_Spells.statSO, m_data_Spells.activeSpellSO[(int)Spells.BaseArcher], m_pool_BaseArcher, m_parent[(int)Spells.BaseArcher]);
                        break;

                    case (int)Spells.NearPlayer:
                        m_spawnScript.SpawnNearPlayer(m_data_Spells.statSO, m_data_Spells.activeSpellSO[(int)Spells.NearPlayer], m_pool_NearPlayer, m_parent[(int)Spells.NearPlayer]);
                        break;

                    case (int)Spells.Aura:
                        break;
                        
                    case (int)Spells.Boomerang:
                        m_spawnScript.SpawnBoomerang(m_data_Spells.statSO, m_data_Spells.activeSpellSO[(int)Spells.Boomerang], m_pool_Boomerang, m_parent[(int)Spells.Boomerang]);
                        break;
                        
                    case (int)Spells.ProtectiveOrbs:
                        m_spawnScript.SpawnProtectiveOrbs(m_data_Spells.statSO, m_data_Spells.activeSpellSO[(int)Spells.ProtectiveOrbs], m_pool_ProtectiveOrbs, m_parent[(int)Spells.ProtectiveOrbs]);
                        break;
                        
                    case (int)Spells.GroundMine:
                        m_spawnScript.SpawnGroundMine(m_data_Spells.statSO, m_data_Spells.activeSpellSO[(int)Spells.GroundMine], m_pool_GroundMine, m_parent[(int)Spells.GroundMine]);
                        break;
                        
                    case (int)Spells.Shockwave:
                        m_spawnScript.SpawnShockwave(m_data_Spells.statSO, m_data_Spells.activeSpellSO[(int)Spells.Shockwave], m_pool_Shockwave, m_parent[(int)Spells.Shockwave]);
                        break;
                        
                    case (int)Spells.Bomb:
                        m_spawnScript.SpawnBomb(m_data_Spells.statSO, m_data_Spells.activeSpellSO[(int)Spells.Bomb], m_pool_Bomb, m_parent[(int)Spells.Bomb]);
                        break;
                        
                    case (int)Spells.PoisonArea:
                        m_spawnScript.SpawnPoisonArea(m_data_Spells.statSO, m_data_Spells.activeSpellSO[(int)Spells.PoisonArea], m_pool_PoisonArea, m_parent[(int)Spells.PoisonArea]);
                        break;
                        
                    case (int)Spells.ChainLightning:
                        m_spawnScript.SpawnChainLightning(m_data_Spells.statSO, m_data_Spells.activeSpellSO[(int)Spells.ChainLightning], m_pool_ChainLightning, m_parent[(int)Spells.ChainLightning]);
                        break;
                        
                    case (int)Spells.ArrowVolley:
                        m_spawnScript.SpawnArrowVolley(m_data_Spells.statSO, m_data_Spells.activeSpellSO[(int)Spells.ArrowVolley], m_pool_ArrowVolley, m_parent[(int)Spells.ArrowVolley]);
                        break;
                        
                    case (int)Spells.Shield:
                        m_parent[(int)Spells.Shield].gameObject.SetActive(true);
                        m_parent[(int)Spells.Shield].GetComponent<Spell_Shield>().OnSpawn(m_data_Spells.statSO, m_data_Spells.activeSpellSO[(int)Spells.Shield]);
                        break;
                }
                m_timer[i] = 0;             // reset the timer
            }
        }
    }

    /// <summary>
    /// Either upgrade a learned spell or learn a new spell
    /// </summary>
    /// <param name="_spell"></param>
    public void ChooseNewSpell(Spells _spell)
    {
        if (m_spellUIScript == null) m_spellUIScript = FindObjectOfType<ShowSpells>();

        // check if its an active spell
        if ((int)_spell < (int)Spells.ActiveSpells)
        {
            // if the spell is already learned (level above 0), upgrade it
            if (m_data_Spells.activeSpellSO[(int)_spell].Level != 0)
                UpgradeActiveSpell(_spell);

            // else learn it
            else
            {
                LearnActiveSpell(_spell);
                m_spellUIScript.LearnActiveSpell(_spell);
            }
        }

        // check if its an passive spell
        else if ((int)_spell > (int)Spells.ActiveSpells && (int)_spell < (int)Spells.PassiveSpells)
        {
            // get the index of the passive spell ("delete" active Spells for index)
            int idx = (int)_spell - ((int)Spells.ActiveSpells + 1);

            // if the spell is already learned (level above 0), upgrade it
            if (m_data_Spells.passiveSpellSO[idx].Level != 0)
                UpgradePassiveSpell(_spell);

            // else learn it
            else
            {
                LearnPassiveSpell(_spell);
                m_spellUIScript.LearnPassiveSpell(_spell);
            }
        }
    }

    #region LearnNewSpell

    /// <summary>
    /// Set Level, active and CD values for the spell,create an Object Pool and parent object
    /// </summary>
    /// <param name="_spell"></param>
    private void LearnActiveSpell(Spells _spell)
    {
        SO_ActiveSpells spellSO = m_data_Spells.activeSpellSO[(int)_spell];

        spellSO.Level = 1;                                              // set Spell Level to 1
        m_active[(int)_spell] = true;                                   // set Spell as active
        m_timer[(int)_spell] = spellSO.Cd[spellSO.Level - 1] - 0.2f;    // set Timer to nearly finish, to instantly fire it 

        GameObject obj = new GameObject();

        switch (_spell)
        {
            case Spells.BaseArcher:
                m_pool_BaseArcher = new ObjectPool<Spell_BaseArcher>(m_prefab_BaseArcher);
                obj.name = "BaseArcher";
                break;

            case Spells.AllDirections:
                m_pool_AllDirections = new ObjectPool<Spell_AllDirections>(m_prefab_AllDirections);
                obj.name = "AllDirections";
                break;

            case Spells.NearPlayer:
                m_pool_NearPlayer = new ObjectPool<Spell_NearPlayer>(m_prefab_NearPlayer);
                obj.name = "NearPlayer";
                break;

            case Spells.Aura:
                GameObject auraObj = Instantiate(m_prefab_Aura, FindObjectOfType<PlayerController>().gameObject.transform);
                auraObj.name = "Aura";
                auraObj.transform.localScale = new Vector3
                    (spellSO.Radius[spellSO.Level - 1], spellSO.Radius[spellSO.Level - 1], spellSO.Radius[spellSO.Level - 1]);
                m_parent[(int)_spell] = auraObj.transform;
                m_parent[(int)_spell].GetComponent<Spell_Aura>().OnSpawn(m_data_Spells.statSO, spellSO);
                return;

            case Spells.Boomerang:
                m_pool_Boomerang = new ObjectPool<Spell_Boomerang>(m_prefab_Boomerang);
                obj.name = "Boomerang";
                break;

            case Spells.ProtectiveOrbs:
                m_pool_ProtectiveOrbs = new ObjectPool<Spell_ProtectiveOrbs>(m_prefab_ProtectiveOrbs);
                obj.name = "ProtectiveOrbs";
                break;

            case Spells.GroundMine:
                m_pool_GroundMine = new ObjectPool<Spell_GroundMine>(m_prefab_GroundMine);
                obj.name = "GroundMine";
                break;
                
            case Spells.Shockwave:
                m_pool_Shockwave = new ObjectPool<Spell_Shockwave>(m_prefab_Shockwave);
                obj.name = "Shockwave";
                break;
                
            case Spells.Bomb:
                m_pool_Bomb = new ObjectPool<Spell_Bomb>(m_prefab_Bomb);
                obj.name = "Bomb";
                break;
                
            case Spells.PoisonArea:
                m_pool_PoisonArea = new ObjectPool<Spell_PoisonArea>(m_prefab_PoisonArea);
                obj.name = "PoisonArea";
                break;
                
            case Spells.ChainLightning:
                m_pool_ChainLightning = new ObjectPool<Spell_ChainLightning>(m_prefab_ChainLightning);
                obj.name = "ChainLightning";
                break;
                
            case Spells.ArrowVolley:
                m_pool_ArrowVolley = new ObjectPool<Spell_ArrowVolley>(m_prefab_ArrowVolley);
                obj.name = "ArrowVolley";
                break;
                
            case Spells.Shield:
                GameObject shieldObj = Instantiate(m_prefab_Shield, FindObjectOfType<PlayerController>().gameObject.transform);
                shieldObj.name = "Shield";
                shieldObj.transform.localScale = new Vector3(1f, 1f, 1f);
                m_parent[(int)_spell] = shieldObj.transform;
                return;
        }

        // Set Parent object 
        obj.transform.SetParent(m_parent_Spells.transform);
        m_parent[(int)_spell] = obj.transform;
    }

    /// <summary>
    /// Increase the Passive Stat in the Player Scriptable Object and set the level to 1
    /// </summary>
    /// <param name="_spell"></param>
    private void LearnPassiveSpell(Spells _spell)
    {
        SO_PassiveSpells spellSO = m_data_Spells.passiveSpellSO[(int)_spell - ((int)Spells.ActiveSpells + 1)];

        spellSO.Level = 0;

        UpgradePassiveSpell(_spell);
    }

    #endregion

    #region UpgradeSpell

    /// <summary>
    /// Increase Level
    /// </summary>
    private void UpgradeActiveSpell(Spells _spell)
    {
        SO_ActiveSpells spellSO = m_data_Spells.activeSpellSO[(int)_spell];

        spellSO.Level++;

        switch (_spell)
        {
            case Spells.Aura:
                if (spellSO.Radius.Length >= spellSO.Level)
                    m_parent[(int)_spell].transform.localScale = new Vector3
                            (spellSO.Radius[spellSO.Level - 1], spellSO.Radius[spellSO.Level - 1], spellSO.Radius[spellSO.Level - 1]);
                break;
        }
    }

    /// <summary>
    /// Increase Level and increase the stat in player
    /// </summary>
    private void UpgradePassiveSpell(Spells _spell)
    {
        SO_PassiveSpells spellSO = m_data_Spells.passiveSpellSO[(int)_spell - ((int)Spells.ActiveSpells + 1)];

        spellSO.Level++;

        // Increase Stat in Player
        switch (_spell)
        {
            case Spells.MovementSpeed:
                m_data_Spells.statSO.MovementSpeed += spellSO.Stat[spellSO.Level - 1];
                break;

            case Spells.DamageMultiplier:
                m_data_Spells.statSO.DamageMultiplier += spellSO.Stat[spellSO.Level - 1];
                break;

            case Spells.CritChance:
                m_data_Spells.statSO.CritChance += spellSO.Stat[spellSO.Level - 1];
                break;

            case Spells.CritMultiplier:
                m_data_Spells.statSO.CritMultiplier += spellSO.Stat[spellSO.Level - 1];
                break;

            case Spells.AreaMultiplier:
                m_data_Spells.statSO.AreaMultiplier += spellSO.Stat[spellSO.Level - 1];
                break;

            case Spells.MaxHealth:
                m_data_Spells.statSO.MaxHealth += spellSO.Stat[spellSO.Level - 1];
                break;

            case Spells.CdReduction:
                m_data_Spells.statSO.CdReduction += spellSO.Stat[spellSO.Level - 1];
                break;

            case Spells.HealthRegeneration:
                m_data_Spells.statSO.HealthRegeneration += spellSO.Stat[spellSO.Level - 1];
                break;

            case Spells.DamageReductionPercentage:
                m_data_Spells.statSO.DamageReductionPercentage += spellSO.Stat[spellSO.Level - 1];
                break;

            case Spells.CollectionRadius:
                m_data_Spells.statSO.CollectionRadius += spellSO.Stat[spellSO.Level - 1];
                FindObjectOfType<LevelPlayer>().UpdateCollectionRadius();
                break;

            case Spells.XPMultiplier:
                m_data_Spells.statSO.XPMultiplier += spellSO.Stat[spellSO.Level - 1];
                break;
        }
    }

    #endregion
}
