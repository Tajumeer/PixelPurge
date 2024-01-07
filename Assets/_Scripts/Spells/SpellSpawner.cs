using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class SpellSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spellPrefab;
    private ObjectPool<Spells> spellPool;
    private GameObject spellParent;

    private PlayerController player;

    [Header("Scriptable Objects")]
    [HideInInspector] public SO_Spells data_AllDirections;
    [HideInInspector] public SO_Spells data_NearPlayer;
    [HideInInspector] public SO_Spells data_BaseArcher;

    [Header("Active Spells")]
    [HideInInspector] public bool active_AllDirections = false;
    [HideInInspector] public bool active_NearPlayer = false;
    [HideInInspector] public bool active_BaseArcher = false;

    [Header("Spell Timer")]
    private float timer_AllDirections = 0f;
    private float timer_NearPlayer = 0f;
    private float timer_BaseArcher = 0f;


    void OnEnable()
    {
        player = FindObjectOfType<PlayerController>();

        // Create Spell Parent GameObject
        spellPool = new ObjectPool<Spells>(spellPrefab);
        spellParent = new GameObject();
        spellParent.name = "Spells";
    }

    private void Update()
    {
        // ALL DIRECTIONS
        if (active_AllDirections)
        {
            timer_AllDirections += Time.deltaTime;
            if(timer_AllDirections >= data_AllDirections.cd)
            {
                SpawnAllDirections();
                timer_AllDirections = 0;
            }
        }

        // NEAR PLAYER
        if (active_NearPlayer)
        {
            timer_NearPlayer += Time.deltaTime;
            if (timer_NearPlayer >= data_NearPlayer.cd)
            {
                SpawnNearPlayer();
                timer_NearPlayer = 0;
            }
        }

        // BASE ARCHER
        if (active_BaseArcher)
        {
            timer_BaseArcher += Time.deltaTime;
            if (timer_BaseArcher >= data_BaseArcher.cd)
            {
                SpawnBaseArcher();
                timer_BaseArcher = 0;
            }
        }
    }

    /// <summary>
    /// Spawn the SpellPrefab, set the tag and parent and reset transform
    /// </summary>
    private Spells SpawnPrefab()
    {
        Spells spellObj = spellPool.GetObject();     // get an object from the pool or a new one 

        if(spellObj.tag != "PlayerSpell")
        {
            spellObj.transform.SetParent(spellParent.transform);
            spellObj.tag = "PlayerSpell";
        }
        
        spellObj.ResetObj(player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

        return spellObj;
    }

    private void SpawnBaseArcher()
    {

        for (int i = 0; i < data_NearPlayer.projectileData.amount; i++)
        {
            Spells spellObj = SpawnPrefab();

            Spell_BaseArcher spellScript;
            if (spellObj.gameObject.GetComponent<Spell_BaseArcher>() != null)
                spellScript = spellObj.gameObject.GetComponent<Spell_BaseArcher>();
            else
                spellScript = spellObj.gameObject.AddComponent<Spell_BaseArcher>();

            // copy the spellScript values and disable it to use the specified spell script
            spellScript.enabled = true;
            spellScript.Init(spellObj.pool);
            spellObj.enabled = false;

            spellScript.OnSpawn(i, data_BaseArcher);
        }
    }

    /// <summary>
    /// Spawn the Spell "AllDirections"
    /// </summary>
    private void SpawnAllDirections()
    {
        for (int i = 0; i < data_AllDirections.projectileData.amount; i++)
        {
            Spells spellObj = SpawnPrefab();

            Spell_AllDirections spellScript;
            if (spellObj.gameObject.GetComponent<Spell_AllDirections>() != null)
                spellScript = spellObj.gameObject.GetComponent<Spell_AllDirections>();
            else
                spellScript = spellObj.gameObject.AddComponent<Spell_AllDirections>();

            spellScript.enabled = true;
            spellScript.Init(spellObj.pool);
            spellObj.enabled = false;

            spellScript.OnSpawn(i, data_AllDirections);
        }
    }

    /// <summary>
    /// Spawn the Spell "NearPlayer"
    /// </summary>
    private void SpawnNearPlayer()
    {
        for (int i = 0; i < data_NearPlayer.projectileData.amount; i++)
        { 
            Spells spellObj = SpawnPrefab();

            Spell_NearPlayer spellScript;
            if(spellObj.gameObject.GetComponent<Spell_NearPlayer>() != null)
                spellScript = spellObj.gameObject.GetComponent<Spell_NearPlayer>();
            else
                spellScript = spellObj.gameObject.AddComponent<Spell_NearPlayer>();

            spellScript.enabled = true;
            spellScript.Init(spellObj.pool);
            spellObj.enabled = false;

            spellScript.OnSpawn(i, data_NearPlayer);
        }
    }
}
