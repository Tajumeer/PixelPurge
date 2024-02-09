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
    ActiveSpells,
    MaxHealth,
    PassiveSpells,
    SpellAmount
}

public class SpellManager : MonoBehaviour
{
    SpellSpawner spawnScript;
    ShowSpells spellUIScript;
    PlayerStats passiveData;

    [Header("Scriptable Objects")]
    [SerializeField] private SO_AllSpells m_data_Spells;
    [SerializeField] private SO_PassiveData m_data_Passives;
    [Space]
    [SerializeField] private SO_Spells m_data_BaseArcher;
    [SerializeField] private SO_Spells m_data_AllDirections;
    [SerializeField] private SO_Spells m_data_NearPlayer;
    [SerializeField] private SO_Spells m_data_Aura;

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

        m_data_Spells.spellSO = new SO_Spells[(int)Spells.SpellAmount];

        // Instantiate new Scriptable Objects for this run
        for (int i = 0; i < (int)Spells.SpellAmount; i++)
        {
            switch ((Spells)i)
            {
                case Spells.BaseArcher:
                    m_data_Spells.spellSO[i] = Instantiate(m_data_BaseArcher);
                    break;

                case Spells.AllDirections:
                    m_data_Spells.spellSO[i] = Instantiate(m_data_AllDirections);
                    break;

                case Spells.NearPlayer:
                    m_data_Spells.spellSO[i] = Instantiate(m_data_NearPlayer);
                    break;

                case Spells.Aura:
                    m_data_Spells.spellSO[i] = Instantiate(m_data_Aura);
                    break;

            }
        }

        // Prototype
        LearnSpell(Spells.BaseArcher);
    }

    public void InitPassives(PlayerController _controller)
    {
        //passiveData = _controller.ActivePlayerData;
    }

    private void Update()
    {
        for (int i = 0; i < (int)Spells.SpellAmount; i++)
        {
            // if at this place is no spell skip
            if (m_data_Spells.spellSO[i] == null) continue;
            if (!m_active[i]) continue;     // skip if spell is not learned

            m_timer[i] += Time.deltaTime;
            if (m_timer[i] >= m_cd[i])      // if spell is ready
            {
                switch (i)                  // check which spell it was and spawn it
                {
                    case (int)Spells.AllDirections:
                        spawnScript.SpawnAllDirections(m_data_Spells.spellSO[(int)Spells.AllDirections], m_pool_AllDirections, m_parent[(int)Spells.AllDirections]);
                        break;

                    case (int)Spells.BaseArcher:
                        spawnScript.SpawnBaseArcher(m_data_Spells.spellSO[(int)Spells.BaseArcher], m_pool_BaseArcher, m_parent[(int)Spells.BaseArcher]);
                        break;

                    case (int)Spells.NearPlayer:
                        spawnScript.SpawnNearPlayer(m_data_Spells.spellSO[(int)Spells.NearPlayer], m_pool_NearPlayer, m_parent[(int)Spells.NearPlayer]);
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

        if (m_data_Spells.spellSO[(int)_spell].Level != 0)
            UpgradeSpell(_spell);
        else
        {
            LearnSpell(_spell);
            spellUIScript.LearnNewSpell(_spell);
        }
    }

    /// <summary>
    /// Learn the Spell "Base Archer" and show it in UI
    /// </summary>
    private void LearnSpell(Spells _spell)
    {
        SO_Spells spellSO = m_data_Spells.spellSO[(int)_spell];

        spellSO.Level = 1;                                      // set Spell Level to 1
        m_active[(int)_spell] = true;                           // set Spell as active
        m_cd[(int)_spell] = spellSO.Cd[spellSO.Level - 1];      // set Timer

        GameObject obj = new GameObject();

        // create Object Pool for the Spell and its name
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
                // Create Spell Parent GameObject 
                GameObject auraObj = Instantiate(m_prefab_Aura, FindObjectOfType<PlayerController>().gameObject.transform);
                auraObj.name = "Aura";
                m_parent[(int)_spell] = auraObj.transform;
                auraObj.transform.localScale = new Vector3
                    (spellSO.Radius[spellSO.Level - 1], spellSO.Radius[spellSO.Level - 1], spellSO.Radius[spellSO.Level - 1]);
                m_parent[(int)_spell] = obj.transform;
                auraObj.GetComponent<Spell_Aura>().OnSpawn(spellSO);
                break;


            case Spells.MaxHealth:
                m_data_Passives.MaxHealth_Level = 1;
                passiveData.MaxHealth += m_data_Passives.MaxHealth[m_data_Passives.MaxHealth_Level - 1];
                break;

        }

        // Set Parent object for later use
        obj.transform.SetParent(m_parent_Spells.transform);
        m_parent[(int)_spell] = obj.transform;
    }


    private void UpgradeSpell(Spells _spell)
    {
        SO_Spells spellSO = m_data_Spells.spellSO[(int)_spell];
        spellSO.Level++;

        switch (_spell)
        {
            case Spells.Aura:
                m_parent[(int)_spell].transform.localScale = new Vector3
                        (spellSO.Radius[spellSO.Level - 1], spellSO.Radius[spellSO.Level - 1], spellSO.Radius[spellSO.Level - 1]);
                break;

            case Spells.MaxHealth:
                passiveData.MaxHealth += m_data_Passives.MaxHealth[m_data_Passives.MaxHealth_Level - 1];
                m_data_Passives.MaxHealth_Level++;
                break;
        }
    }
}
