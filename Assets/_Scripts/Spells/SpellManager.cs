using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class SpellManager : MonoBehaviour
{
    SpellSpawner spawnScript;

    [Header("Scriptable Objects")]
    [SerializeField] private SO_SpellUpgrades data_Upgrades;
    [Space]
    [SerializeField] private SO_AllDirections data_AllDirections_original;
    [SerializeField] private SO_NearPlayer data_NearPlayer_original;
    [SerializeField] private SO_BaseArcher data_BaseArcher_original;

    private SO_AllDirections data_AllDirections;
    private SO_NearPlayer data_NearPlayer;
    private SO_BaseArcher data_BaseArcher;

    [Header("Active Spells")]
    [HideInInspector] public bool active_AllDirections = false;
    [HideInInspector] public bool active_NearPlayer = false;
    [HideInInspector] public bool active_BaseArcher = false;
    [HideInInspector] public bool active_Aura = false;

    [Header("Spell Timer")]
    private float timer_AllDirections = 0f;
    private float timer_NearPlayer = 0f;
    private float timer_BaseArcher = 0f;

    [Header("Prefabs")]
    [SerializeField] private GameObject prefab_AllDirections;
    [SerializeField] private GameObject prefab_NearPlayer;
    [SerializeField] private GameObject prefab_BaseArcher;
    [SerializeField] private GameObject prefab_Aura;

    [Header("Pools")]
    [HideInInspector] public ObjectPool<Spell_AllDirections> pool_AllDirections;
    [HideInInspector] public ObjectPool<Spell_NearPlayer> pool_NearPlayer;
    [HideInInspector] public ObjectPool<Spell_BaseArcher> pool_BaseArcher;

    [Header("Parent Objects")]
    [HideInInspector] public Transform parent_Spells;
    [HideInInspector] public Transform parent_AllDirections;
    [HideInInspector] public Transform parent_NearPlayer;
    [HideInInspector] public Transform parent_BaseArcher;

    [Header("Spell Levels")]
    private int level_AllDirections = 1;
    private int level_NearPlayer = 1;

    private void OnEnable()
    {
        spawnScript = FindObjectOfType<SpellSpawner>();

        // Create Spell Parent GameObject
        GameObject temp = new GameObject();
        temp.name = "Spells";
        parent_Spells = temp.transform;

        // Prototype
        LearnBaseArcher();
        LearnAllDirections();
        LearnNearPlayer();
    }

    private void Update()
    {
        // ALL DIRECTIONS
        if (active_AllDirections)
        {
            timer_AllDirections += Time.deltaTime;
            if (timer_AllDirections >= data_AllDirections.Cd)
            {
                spawnScript.SpawnAllDirections(data_AllDirections, pool_AllDirections, parent_AllDirections);
                timer_AllDirections = 0;
            }
        }

        // NEAR PLAYER
        if (active_NearPlayer)
        {
            timer_NearPlayer += Time.deltaTime;
            if (timer_NearPlayer >= data_NearPlayer.Cd)
            {
                spawnScript.SpawnNearPlayer(data_NearPlayer, pool_NearPlayer, parent_NearPlayer);
                timer_NearPlayer = 0;
            }
        }

        // BASE ARCHER
        if (active_BaseArcher)
        {
            timer_BaseArcher += Time.deltaTime;
            if (timer_BaseArcher >= data_BaseArcher.Cd)
            {
                spawnScript.SpawnBaseArcher(data_BaseArcher, pool_BaseArcher, parent_BaseArcher);
                timer_BaseArcher = 0;
            }
        }
    }

    #region Learn New Spells

    /// <summary>
    /// Learn the Spell "AllDirections" and show it in UI
    /// </summary>
    public void LearnBaseArcher()
    {
        data_BaseArcher = Instantiate(data_BaseArcher_original);
        //spawnScript.data_BaseArcher = Instantiate(data_BaseArcher_original);
        //spawnScript.active_BaseArcher = true;
        active_BaseArcher = true;

        // Create ObjectPool
        pool_BaseArcher = new ObjectPool<Spell_BaseArcher>(prefab_BaseArcher);

        // Create Spell Parent GameObject 
        GameObject temp = new GameObject();
        temp.name = "BaseArcher";
        temp.transform.SetParent(parent_Spells.transform);
        parent_BaseArcher = temp.transform;
    }

    /// <summary>
    /// Learn the Spell "AllDirections" and show it in UI
    /// </summary>
    public void LearnAllDirections()
    {
        level_AllDirections = 1;
        data_AllDirections = Instantiate(data_AllDirections_original);
        //spawnScript.data_AllDirections = Instantiate(data_AllDirections_original);
        //spawnScript.active_AllDirections = true;
        active_AllDirections = true;

        // Create ObjectPool
        pool_AllDirections = new ObjectPool<Spell_AllDirections>(prefab_AllDirections);

        // Create Spell Parent GameObject 
        GameObject temp = new GameObject();
        temp.name = "AllDirections";
        temp.transform.SetParent(parent_Spells.transform);
        parent_AllDirections = temp.transform;

        // UI
    }

    /// <summary>
    /// Learn the Spell "NearPlayer" and show it in UI
    /// </summary>
    public void LearnNearPlayer()
    {
        level_NearPlayer = 1;
        data_NearPlayer = Instantiate(data_NearPlayer_original);
        //spawnScript.data_NearPlayer = Instantiate(data_NearPlayer_original);
        //spawnScript.active_NearPlayer = true;
        active_NearPlayer = true;

        // Create ObjectPool
        pool_NearPlayer = new ObjectPool<Spell_NearPlayer>(prefab_NearPlayer);

        // Create Spell Parent GameObject 
        GameObject temp = new GameObject();
        temp.name = "NearPlayer";
        temp.transform.SetParent(parent_Spells.transform);
        parent_NearPlayer = temp.transform;

        // UI
    }

    #endregion

    #region Upgrade Spells

    /// <summary>
    /// Upgrade values of the spell and update UI
    /// </summary>
    public void UpradeAllDirections()
    {
        //spawnScript.data_AllDirections.damage *= (1f + data_Upgrades_original.Damage[level_AllDirections]);
        data_AllDirections.Damage *= (1f + data_Upgrades.Damage[level_AllDirections]);

        level_AllDirections++;

        // UI
    }

    /// <summary>
    /// Upgrade values of the spell and update UI
    /// </summary>
    public void UpgradeNearPlayer()
    {
        //spawnScript.data_NearPlayer.damage *= (1f + data_Upgrades_original.Damage[level_NearPlayer]);
        data_NearPlayer.Damage *= (1f + data_Upgrades.Damage[level_NearPlayer]);

        level_NearPlayer++;

        // UI
    }

    #endregion
}
