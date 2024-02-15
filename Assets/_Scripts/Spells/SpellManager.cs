using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// Maya

/// <summary>
/// First Active Spells then Passive Spells, at last "SpellAmount".
/// Where to edit them manually: SpellManager
/// </summary>
[Serializable]
public enum Spells
{
    BaseArcher = 0,
    AllDirections,
    NearPlayer,
    Aura,
    /// <summary> Indicator: End of Active Spells </summary>
    ActiveSpells,

    MovementSpeed,
    DamageMultiplier,
    CritChance,
    CritMultiplier,
    AttackSpeed,
    AreaMultiplier,
    RecastTimeMultiplier,
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
    PlayerStats m_passiveData;

    [Header("Scriptable Objects")]
    [SerializeField] private SO_AllSpells m_data_Spells;
    [SerializeField] private SO_OriginalSpells m_data_OriginalSpells;

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
    private Transform[] m_parent = new Transform[(int)Spells.ActiveSpells];
    private Transform m_parent_Spells;

    private bool[] m_active = new bool[(int)Spells.ActiveSpells];

    private float[] m_timer = new float[(int)Spells.ActiveSpells];

    private float[] m_cd = new float[(int)Spells.ActiveSpells];

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

        // Prototype
        LearnActiveSpell(Spells.BaseArcher);
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

                case Spells.AttackSpeed:
                    m_data_Spells.passiveSpellSO[idx] = Instantiate(m_data_OriginalSpells.Data_AttackSpeed);
                    break;

                case Spells.AreaMultiplier:
                    m_data_Spells.passiveSpellSO[idx] = Instantiate(m_data_OriginalSpells.Data_AreaMultiplier);
                    break;

                case Spells.RecastTimeMultiplier:
                    m_data_Spells.passiveSpellSO[idx] = Instantiate(m_data_OriginalSpells.Data_RecastTimeMultiplier);
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

    public void InitPassiveData(PlayerController _controller)
    {
        m_passiveData = _controller.ActivePlayerData;
    }

    private void Update()
    {
        for (int i = 0; i < (int)Spells.ActiveSpells; i++)
        {
            if (m_data_Spells.activeSpellSO[i] == null) continue;   // safety check if its a spell
            if (!m_active[i]) continue;     // skip if spell is not learned

            m_timer[i] += Time.deltaTime;
            if (m_timer[i] >= m_cd[i])      // if spell is ready
            {
                switch (i)                  // check which spell it was and spawn it
                {
                    case (int)Spells.AllDirections:
                        m_spawnScript.SpawnAllDirections(m_data_Spells.activeSpellSO[(int)Spells.AllDirections], m_pool_AllDirections, m_parent[(int)Spells.AllDirections]);
                        break;

                    case (int)Spells.BaseArcher:
                        m_spawnScript.SpawnBaseArcher(m_data_Spells.activeSpellSO[(int)Spells.BaseArcher], m_pool_BaseArcher, m_parent[(int)Spells.BaseArcher]);
                        break;

                    case (int)Spells.NearPlayer:
                        m_spawnScript.SpawnNearPlayer(m_data_Spells.activeSpellSO[(int)Spells.NearPlayer], m_pool_NearPlayer, m_parent[(int)Spells.NearPlayer]);
                        break;

                    case (int)Spells.Aura:

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

        spellSO.Level = 1;                                      // set Spell Level to 1
        m_active[(int)_spell] = true;                           // set Spell as active
        m_cd[(int)_spell] = spellSO.Cd[spellSO.Level - 1];      // set Timer

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
                m_parent[(int)_spell] = auraObj.transform;
                auraObj.transform.localScale = new Vector3
                    (spellSO.Radius[spellSO.Level - 1], spellSO.Radius[spellSO.Level - 1], spellSO.Radius[spellSO.Level - 1]);
                m_parent[(int)_spell] = obj.transform;
                auraObj.GetComponent<Spell_Aura>().OnSpawn(spellSO);
                break;
        }

        // Set Parent object for later use
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
                m_passiveData.MovementSpeed += spellSO.Stat[spellSO.Level - 1];
                break;

            case Spells.DamageMultiplier:
                m_passiveData.DamageMultiplier += spellSO.Stat[spellSO.Level - 1];
                break;

            case Spells.CritChance:
                m_passiveData.CritChance += spellSO.Stat[spellSO.Level - 1];
                break;

            case Spells.CritMultiplier:
                m_passiveData.CritMultiplier += spellSO.Stat[spellSO.Level - 1];
                break;

            case Spells.AttackSpeed:
                m_passiveData.AttackSpeed += spellSO.Stat[spellSO.Level - 1];
                break;

            case Spells.AreaMultiplier:
                m_passiveData.AreaMultiplier += spellSO.Stat[spellSO.Level - 1];
                break;

            case Spells.MaxHealth:
                m_passiveData.MaxHealth += spellSO.Stat[spellSO.Level - 1];
                break;

            case Spells.RecastTimeMultiplier:
                m_passiveData.RecastTimeMultiplier += spellSO.Stat[spellSO.Level - 1];
                break;

            case Spells.HealthRegeneration:
                m_passiveData.HealthRegeneration += spellSO.Stat[spellSO.Level - 1];
                break;

            case Spells.DamageReductionPercentage:
                m_passiveData.DamageReductionPercentage += spellSO.Stat[spellSO.Level - 1];
                break;

            case Spells.CollectionRadius:
                m_passiveData.CollectionRadius += spellSO.Stat[spellSO.Level - 1];
                break;

            case Spells.XPMultiplier:
                m_passiveData.XPMultiplier += spellSO.Stat[spellSO.Level - 1];
                break;
        }
    }

    #endregion
}
