using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class SpellSpawner : MonoBehaviour
{
    private PlayerController player;

    //[Header("Scriptable Objects")]
    //[HideInInspector] public SO_AllDirections data_AllDirections;
    //[HideInInspector] public SO_NearPlayer data_NearPlayer;
    //[HideInInspector] public SO_BaseArcher data_BaseArcher;
    //[HideInInspector] public SO_Aura data_Aura;

    //[Header("Prefabs")]
    //[SerializeField] private GameObject prefab_AllDirections;
    //[SerializeField] private GameObject prefab_NearPlayer;
    //[SerializeField] private GameObject prefab_BaseArcher;
    //[SerializeField] private GameObject prefab_Aura;

    //[Header("Pools")]
    //[HideInInspector] public ObjectPool<Spells_old> pool_AllDirections;
    //[HideInInspector] public ObjectPool<Spells_old> pool_NearPlayer;
    //[HideInInspector] public ObjectPool<Spells_old> pool_BaseArcher;

    //[Header("Parent Objects")]
    //[HideInInspector] public GameObject parent_AllDirections;
    //[HideInInspector] public GameObject parent_NearPlayer;
    //[HideInInspector] public GameObject parent_BaseArcher;

    //[Header("Active Spells")]
    //[HideInInspector] public bool active_AllDirections = false;
    //[HideInInspector] public bool active_NearPlayer = false;
    //[HideInInspector] public bool active_BaseArcher = false;
    //[HideInInspector] public bool active_Aura = false;

    //[Header("Spell Timer")]
    //private float timer_AllDirections = 0f;
    //private float timer_NearPlayer = 0f;
    //private float timer_BaseArcher = 0f;


    void OnEnable()
    {
        player = FindObjectOfType<PlayerController>();

        //// Create Spell Parent GameObject
        //spellPool = new ObjectPool<Spells>(spellPrefab);
        //spellParent = new GameObject();
        //spellParent.name = "Spells";
    }

    //private void Update()
    //{
    //    // ALL DIRECTIONS
    //    if (active_AllDirections)
    //    {
    //        timer_AllDirections += Time.deltaTime;
    //        if (timer_AllDirections >= data_AllDirections.cd)
    //        {
    //            SpawnAllDirections();
    //            timer_AllDirections = 0;
    //        }
    //    }

    //    // NEAR PLAYER
    //    if (active_NearPlayer)
    //    {
    //        timer_NearPlayer += Time.deltaTime;
    //        if (timer_NearPlayer >= data_NearPlayer.cd)
    //        {
    //            SpawnNearPlayer();
    //            timer_NearPlayer = 0;
    //        }
    //    }

    //    // BASE ARCHER
    //    if (active_BaseArcher)
    //    {
    //        timer_BaseArcher += Time.deltaTime;
    //        if (timer_BaseArcher >= data_BaseArcher.cd)
    //        {
    //            SpawnBaseArcher();
    //            timer_BaseArcher = 0;
    //        }
    //    }
    //}

    /// <summary>
    /// Spawn the SpellPrefab, set the tag and parent and reset transform
    /// </summary>
    //private Spells_old SpawnPrefab()
    //{
    //    Spells_old spellObj = spellPool.GetObject();     // get an object from the pool or a new one 

    //    if (spellObj.tag != "PlayerSpell")
    //    {
    //        spellObj.transform.SetParent(spellParent.transform);
    //        spellObj.tag = "PlayerSpell";
    //    }

    //    spellObj.ResetObj(player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

    //    return spellObj;
    //}

    public void SpawnBaseArcher(SO_BaseArcher _data, ObjectPool<Spell_BaseArcher> _pool, Transform _parent)
    {
        for (int i = 0; i < _data.Amount; i++)
        {
            Spell_BaseArcher spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_data);
        }
    }

    /// <summary>
    /// Spawn the Spell "AllDirections"
    /// </summary>
    public void SpawnAllDirections(SO_AllDirections _data, ObjectPool<Spell_AllDirections> _pool, Transform _parent)
    {
        for (int i = 0; i < _data.Amount; i++)
        {
            Spell_AllDirections spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_data, i);
        }
    }

    /// <summary>
    /// Spawn the Spell "NearPlayer"
    /// </summary>
    public void SpawnNearPlayer(SO_NearPlayer _data, ObjectPool<Spell_NearPlayer> _pool, Transform _parent)
    {
        for (int i = 0; i < _data.Amount; i++)
        {
            Spell_NearPlayer spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_data);
        }
    }
}
