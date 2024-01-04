using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class SpellSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spellPrefab;
    private ObjectPool<Spells> spellPool;
    private GameObject parent_spells;

    private Vector3 playerPosition;

    [Header("Scriptable Objects")]
    [HideInInspector] public SO_Spells data_AllDirections;
    [HideInInspector] public SO_Spells data_NearPlayer;

    [Header("Active Spells")]
    [HideInInspector] public bool active_AllDirections = false;
    [HideInInspector] public bool active_NearPlayer = false;

    [Header("Spell Timer")]
    private float timer_AllDirections = 0f;
    private float timer_NearPlayer = 0f;


    void OnEnable()
    {
        playerPosition = new Vector3(0f, 0f, 0f);

        // Create Spell Parent GameObject
        spellPool = new ObjectPool<Spells>(spellPrefab);
        parent_spells = new GameObject();
        parent_spells.name = "Spells";
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
    }

    /// <summary>
    /// Spawn the SpellPrefab, set the tag and parent and reset transform
    /// </summary>
    private Spells SpawnPrefab()
    {
        Spells spellObj = spellPool.GetObject();     // get an object from the pool or a new one 

        if(spellObj.tag != "PlayerSpell")
        {
            spellObj.transform.SetParent(parent_spells.transform);
            spellObj.tag = "PlayerSpell";
        }
        spellObj.ResetObj(playerPosition, new Vector3(0f, 0f, 0f));

        return spellObj;
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
