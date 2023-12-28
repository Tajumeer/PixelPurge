using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Maya

public class SpellManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject prefab_fourDirection;

    [Header("Scriptable Objects")]
    [SerializeField] private SO_FourDirection data_fourDirection;

    [Header("Pools")]
    private ObjectPool<Spell_FourDirection> pool_fourDirection;

    [Header("Parents")]
    GameObject parent_spells;
    GameObject parent_fourDirection;

    private Vector3 playerPosition;

    void OnEnable()
    {
        playerPosition = new Vector3(0f, 0f, 0f);

        InitializeAllSpells();

        StartCoroutine(TimerFourDirectionSpell());
    }

    private void InitializeAllSpells()
    {
        parent_spells = new GameObject();
        parent_spells.name = "Spells";

        // fourDirections
        pool_fourDirection = new ObjectPool<Spell_FourDirection>(prefab_fourDirection);
        parent_fourDirection = new GameObject();
        parent_fourDirection.name = "Spell_FourDirection";
        parent_fourDirection.transform.SetParent(parent_spells.transform);
    }

    IEnumerator TimerFourDirectionSpell()
    {
        for (int i = 0; i < 5; i++)
        {
            CastFourDirectionSpell(data_fourDirection);

            yield return new WaitForSeconds(1f);
        }
    }

    public void CastFourDirectionSpell(SO_FourDirection _spellData)
    {
        for (int i = 0; i < 4; i++)
        {
            Spell_FourDirection spell = pool_fourDirection.GetObject();     // get an object from the pool or a new one 

            // set parent, tag and transform
            spell.transform.SetParent(parent_fourDirection.transform);
            spell.tag = "PlayerSpell";
            spell.ResetObj(playerPosition, new Vector3(0f, 0f, 0f));

            switch (i)
            {
                case 0:
                    spell.Spawn(_spellData, Vector2.up);
                    break;
                case 1:
                    spell.Spawn(_spellData, Vector2.down);
                    break;
                case 2:
                    spell.Spawn(_spellData, Vector2.right);
                    break;
                case 3:
                    spell.Spawn(_spellData, Vector2.left);
                    break;
            }
        }
    }
}
