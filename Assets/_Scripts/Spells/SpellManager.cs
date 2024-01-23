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
    [SerializeField] private SO_Aura data_Aura_original;

    private SO_AllDirections data_AllDirections;
    private SO_NearPlayer data_NearPlayer;
    private SO_BaseArcher data_BaseArcher;
    private SO_Aura data_Aura;

    [Header("Active Spells")]
    private bool active_AllDirections = false;
    private bool active_NearPlayer = false;
    private bool active_BaseArcher = false;

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
    private ObjectPool<Spell_AllDirections> pool_AllDirections;
    private ObjectPool<Spell_NearPlayer> pool_NearPlayer;
    private ObjectPool<Spell_BaseArcher> pool_BaseArcher;

    [Header("Parent Objects")]
    private Transform parent_Spells;
    private Transform parent_AllDirections;
    private Transform parent_NearPlayer;
    private Transform parent_BaseArcher;
    private Transform parent_Aura;

    [Header("Spell Levels")]
    private int level_AllDirections = 1;
    private int level_NearPlayer = 1;
    private int level_Aura = 1;

    private void OnEnable()
    {
        spawnScript = FindObjectOfType<SpellSpawner>();

        // Create Spell Parent GameObject
        GameObject obj = new GameObject();
        obj.name = "Spells";
        parent_Spells = obj.transform;

        // Prototype
        LearnBaseArcher();
        LearnAllDirections();
        LearnNearPlayer();
        LearnAura();
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
        active_BaseArcher = true;

        // Create ObjectPool
        pool_BaseArcher = new ObjectPool<Spell_BaseArcher>(prefab_BaseArcher);

        // Create Spell Parent GameObject 
        GameObject obj = new GameObject();
        obj.name = "BaseArcher";
        obj.transform.SetParent(parent_Spells.transform);
        parent_BaseArcher = obj.transform;
    }

    /// <summary>
    /// Learn the Spell "AllDirections" and show it in UI
    /// </summary>
    public void LearnAllDirections()
    {
        level_AllDirections = 1;
        data_AllDirections = Instantiate(data_AllDirections_original);
        active_AllDirections = true;

        // Create ObjectPool
        pool_AllDirections = new ObjectPool<Spell_AllDirections>(prefab_AllDirections);

        // Create Spell Parent GameObject 
        GameObject obj = new GameObject();
        obj.name = "AllDirections";
        obj.transform.SetParent(parent_Spells.transform);
        parent_AllDirections = obj.transform;

        // UI
    }

    /// <summary>
    /// Learn the Spell "NearPlayer" and show it in UI
    /// </summary>
    public void LearnNearPlayer()
    {
        level_NearPlayer = 1;
        data_NearPlayer = Instantiate(data_NearPlayer_original);
        active_NearPlayer = true;

        // Create ObjectPool
        pool_NearPlayer = new ObjectPool<Spell_NearPlayer>(prefab_NearPlayer);

        // Create Spell Parent GameObject 
        GameObject obj = new GameObject();
        obj.name = "NearPlayer";
        obj.transform.SetParent(parent_Spells.transform);
        parent_NearPlayer = obj.transform;

        // UI
    }

    /// <summary>
    /// Learn the Spell "Aura" and show it in UI
    /// </summary>
    public void LearnAura()
    {
        level_Aura = 1;
        data_Aura = Instantiate(data_Aura_original);

        // Create Spell Parent GameObject 
        GameObject auraObj = Instantiate(prefab_Aura, FindObjectOfType<PlayerController>().gameObject.transform);
        auraObj.name = "Aura";
        parent_Aura = auraObj.transform;

        auraObj.GetComponent<Spell_Aura>().OnSpawn(data_Aura);

        // UI
    }

    #endregion

    #region Upgrade Spells

    /// <summary>
    /// Upgrade values of the spell and update UI
    /// </summary>
    public void UpradeAllDirections()
    {
        data_AllDirections.Damage *= (1f + data_Upgrades.Damage[level_AllDirections]);

        level_AllDirections++;

        // UI
    }

    /// <summary>
    /// Upgrade values of the spell and update UI
    /// </summary>
    public void UpgradeNearPlayer()
    {
        data_NearPlayer.Damage *= (1f + data_Upgrades.Damage[level_NearPlayer]);

        level_NearPlayer++;

        // UI
    }

    /// <summary>
    /// Upgrade values of the spell and update UI
    /// </summary>
    public void UpgradeAura()
    {
        data_Aura.Damage *= (1f + data_Upgrades.Damage[level_Aura]);
        

        level_Aura++;

        // UI
    }

    #endregion
}
