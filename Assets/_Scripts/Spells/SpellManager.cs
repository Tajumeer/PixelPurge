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
    AirWave = 0,
    Aura,
    Shockwave,
    Bomb,
    PoisonArea,
    GroundMine,
    SwordVortex,
    ToxicTrail,
    ShurikenToss,
    HomingRock,
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
    [SerializeField] private GameObject m_prefab_ShurikenToss;
    [SerializeField] private GameObject m_prefab_HomingRock;
    [SerializeField] private GameObject m_prefab_AirWave;
    [SerializeField] private GameObject m_prefab_Aura;
    [SerializeField] private GameObject m_prefab_Boomerang;
    [SerializeField] private GameObject m_prefab_SwordVortex;
    [SerializeField] private GameObject m_prefab_GroundMine;
    [SerializeField] private GameObject m_prefab_Shockwave;
    [SerializeField] private GameObject m_prefab_Bomb;
    [SerializeField] private GameObject m_prefab_PoisonArea;
    [SerializeField] private GameObject m_prefab_ToxicTrail;
    [SerializeField] private GameObject m_prefab_Shield;

    [Header("Pools")]
    private ObjectPool<Spell_ShurikenToss> m_pool_ShurikenToss;
    private ObjectPool<Spell_HomingRock> m_pool_HomingRock;
    private ObjectPool<Spell_AirWave> m_pool_AirWave;
    private ObjectPool<Spell_Boomerang> m_pool_Boomerang;
    private ObjectPool<Spell_SwordVortex> m_pool_SwordVortex;
    private ObjectPool<Spell_GroundMine> m_pool_GroundMine;
    private ObjectPool<Spell_Shockwave> m_pool_Shockwave;
    private ObjectPool<Spell_Bomb> m_pool_Bomb;
    private ObjectPool<Spell_PoisonArea> m_pool_PoisonArea;
    private ObjectPool<Spell_ToxicTrail> m_pool_ToxicTrail;

    [Header("Parent Objects")]
    private Transform[] m_parent = new Transform[(int)Spells.ActiveSpells];
    private Transform m_parent_Spells;

    private bool[] m_active = new bool[(int)Spells.ActiveSpells];

    private float[] m_timer = new float[(int)Spells.ActiveSpells];

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
                case Spells.AirWave:
                    m_data_Spells.activeSpellSO[i] = Instantiate(m_data_OriginalSpells.Data_AirWave);
                    break;

                case Spells.ShurikenToss:
                    m_data_Spells.activeSpellSO[i] = Instantiate(m_data_OriginalSpells.Data_ShurikenToss);
                    break;

                case Spells.HomingRock:
                    m_data_Spells.activeSpellSO[i] = Instantiate(m_data_OriginalSpells.Data_HomingRock);
                    break;

                case Spells.Aura:
                    m_data_Spells.activeSpellSO[i] = Instantiate(m_data_OriginalSpells.Data_Aura);
                    break;

                case Spells.Boomerang:
                    m_data_Spells.activeSpellSO[i] = Instantiate(m_data_OriginalSpells.Data_Boomerang);
                    break;

                case Spells.SwordVortex:
                    m_data_Spells.activeSpellSO[i] = Instantiate(m_data_OriginalSpells.Data_SwordVortex);
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

                case Spells.ToxicTrail:
                    m_data_Spells.activeSpellSO[i] = Instantiate(m_data_OriginalSpells.Data_ToxicTrail);
                    break;

                case Spells.Shield:
                    m_data_Spells.activeSpellSO[i] = Instantiate(m_data_OriginalSpells.Data_Shield);
                    break;
            }
        }

        m_data_Spells.passiveSpellSO = new SO_PassiveSpells[(int)Spells.PassiveSpells - (int)Spells.ActiveSpells - 1];

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

            if (spellSO == null) continue;      // safety check if its a spell
            if (!m_active[i]) continue;         // skip if spell is not learned

            m_timer[i] += Time.deltaTime;
            if (m_timer[i] >= spellSO.Cd[spellSO.Level - 1] * m_data_Spells.statSO.CdReduction)      // if spell is ready
            {
                switch (i)                  // check which spell it was and spawn it
                {
                    case (int)Spells.ShurikenToss:
                        m_spawnScript.SpawnShurikenToss(m_data_Spells.statSO, m_data_Spells.activeSpellSO[(int)Spells.ShurikenToss], m_pool_ShurikenToss, m_parent[(int)Spells.ShurikenToss]);
                        break;

                    case (int)Spells.AirWave:
                        m_spawnScript.SpawnAirWave(m_data_Spells.statSO, m_data_Spells.activeSpellSO[(int)Spells.AirWave], m_pool_AirWave, m_parent[(int)Spells.AirWave]);
                        break;

                    case (int)Spells.HomingRock:
                        m_spawnScript.SpawnHomingRock(m_data_Spells.statSO, m_data_Spells.activeSpellSO[(int)Spells.HomingRock], m_pool_HomingRock, m_parent[(int)Spells.HomingRock]);
                        break;

                    case (int)Spells.Aura:
                        break;

                    case (int)Spells.Boomerang:
                        m_spawnScript.SpawnBoomerang(m_data_Spells.statSO, m_data_Spells.activeSpellSO[(int)Spells.Boomerang], m_pool_Boomerang, m_parent[(int)Spells.Boomerang]);
                        break;

                    case (int)Spells.SwordVortex:
                        m_spawnScript.SpawnSwordVortex(m_data_Spells.statSO, m_data_Spells.activeSpellSO[(int)Spells.SwordVortex], m_pool_SwordVortex, m_parent[(int)Spells.SwordVortex]);
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

                    case (int)Spells.ToxicTrail:
                        m_spawnScript.SpawnToxicTrail(m_data_Spells.statSO, m_data_Spells.activeSpellSO[(int)Spells.ToxicTrail], m_pool_ToxicTrail, m_parent[(int)Spells.ToxicTrail]);
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
            case Spells.AirWave:
                m_pool_AirWave = new ObjectPool<Spell_AirWave>(m_prefab_AirWave);
                obj.name = "AirWave";
                break;

            case Spells.ShurikenToss:
                m_pool_ShurikenToss = new ObjectPool<Spell_ShurikenToss>(m_prefab_ShurikenToss);
                obj.name = "ShurikenToss";
                break;

            case Spells.HomingRock:
                m_pool_HomingRock = new ObjectPool<Spell_HomingRock>(m_prefab_HomingRock);
                obj.name = "HomingRock";
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

            case Spells.SwordVortex:
                m_pool_SwordVortex = new ObjectPool<Spell_SwordVortex>(m_prefab_SwordVortex);
                obj.name = "SwordVortex";
                obj.transform.position = new Vector3(0.2f, -.2f, 0f);
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

            case Spells.ToxicTrail:
                m_pool_ToxicTrail = new ObjectPool<Spell_ToxicTrail>(m_prefab_ToxicTrail);
                obj.name = "ToxicTrail";
                break;

            case Spells.Shield:
                GameObject shieldObj = Instantiate(m_prefab_Shield, FindObjectOfType<PlayerController>().gameObject.transform);
                shieldObj.name = "Shield";
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
                m_data_Spells.statSO.AreaMultiplier *= spellSO.Stat[spellSO.Level - 1];
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
