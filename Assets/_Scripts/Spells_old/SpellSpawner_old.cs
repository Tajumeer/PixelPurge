using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class SpellSpawner_old : MonoBehaviour
{
    [SerializeField] private GameObject spellPrefab;
    private ObjectPool<Spells_old> spellPool;
    private GameObject spellParent;

    private PlayerController player;

    [Header("Scriptable Objects")]
    [HideInInspector] public SO_Spells_old data_AllDirections;
    [HideInInspector] public SO_Spells_old data_NearPlayer;
    [HideInInspector] public SO_Spells_old data_BaseArcher;

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
        spellPool = new ObjectPool<Spells_old>(spellPrefab);
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
    private Spells_old SpawnPrefab()
    {
        Spells_old spellObj = spellPool.GetObject();     // get an object from the pool or a new one 

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
            Spells_old spellObj = SpawnPrefab();

            Spell_BaseArcher_old spellScript;
            if (spellObj.gameObject.GetComponent<Spell_BaseArcher_old>() != null)
                spellScript = spellObj.gameObject.GetComponent<Spell_BaseArcher_old>();
            else
                spellScript = spellObj.gameObject.AddComponent<Spell_BaseArcher_old>();

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
            Spells_old spellObj = SpawnPrefab();

            Spell_AllDirections_old spellScript;
            if (spellObj.gameObject.GetComponent<Spell_AllDirections_old>() != null)
                spellScript = spellObj.gameObject.GetComponent<Spell_AllDirections_old>();
            else
                spellScript = spellObj.gameObject.AddComponent<Spell_AllDirections_old>();

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
            Spells_old spellObj = SpawnPrefab();

            Spell_NearPlayer_old spellScript;
            if(spellObj.gameObject.GetComponent<Spell_NearPlayer_old>() != null)
                spellScript = spellObj.gameObject.GetComponent<Spell_NearPlayer_old>();
            else
                spellScript = spellObj.gameObject.AddComponent<Spell_NearPlayer_old>();

            spellScript.enabled = true;
            spellScript.Init(spellObj.pool);
            spellObj.enabled = false;

            spellScript.OnSpawn(i, data_NearPlayer);
        }
    }
}
