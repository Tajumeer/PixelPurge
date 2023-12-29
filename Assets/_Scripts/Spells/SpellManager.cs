using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Maya

public class SpellManager : MonoBehaviour
{
    [SerializeField] private GameObject spellPrefab;
    private ObjectPool<Spells> spellPool;
    private GameObject parent_spells;

    [Header("Scriptable Objects")]
    [SerializeField] private SO_Spells data_FourDirection;
    [SerializeField] private SO_Spells data_NearPlayer;


    private Vector3 playerPosition;

    void OnEnable()
    {
        playerPosition = new Vector3(0f, 0f, 0f);

        CreateSpellPool();
        StartCoroutine(TimerFourDirectionSpell());
    }

    private void CreateSpellPool()
    {
        // fourDirections
        spellPool = new ObjectPool<Spells>(spellPrefab);
        parent_spells = new GameObject();
        parent_spells.name = "Spells";
    }

    IEnumerator TimerFourDirectionSpell()
    {
        for (int i = 0; i < 5; i++)
        {
            CastFourDirectionSpell(data_FourDirection);
            CastNearPlayerSpell(data_NearPlayer);

            yield return new WaitForSeconds(1f);
        }
    }

    public void CastFourDirectionSpell(SO_Spells data_FourDirection)
    {
        for (int i = 0; i < data_FourDirection.projectileData.amount; i++)
        {
            Spells spell = spellPool.GetObject();     // get an object from the pool or a new one 

            Spell_FourDirection spellScript = spell.gameObject.AddComponent<Spell_FourDirection>();
            spellScript.Init(spell.pool);
            spell.enabled = false;

            // set parent, tag and transform
            spell.transform.SetParent(parent_spells.transform);
            spell.tag = "PlayerSpell";
            spell.ResetObj(playerPosition, new Vector3(0f, 0f, 0f));

            spellScript.OnSpawn(i, data_FourDirection);
                    
        }
    }

    public void CastNearPlayerSpell(SO_Spells data_NearPlayer)
    {
        for (int i = 0; i < data_NearPlayer.projectileData.amount; i++)
        {
            Spells spell = spellPool.GetObject();     // get an object from the pool or a new one 

            Spell_NearPlayer spellScript = spell.gameObject.AddComponent<Spell_NearPlayer>();
            spellScript.Init(spell.pool);
            spell.enabled = false;

            // set parent, tag and transform
            spell.transform.SetParent(parent_spells.transform);
            spell.tag = "PlayerSpell";
            spell.ResetObj(playerPosition, new Vector3(0f, 0f, 0f));

            spellScript.OnSpawn(i, data_NearPlayer);
        }
    }
}
