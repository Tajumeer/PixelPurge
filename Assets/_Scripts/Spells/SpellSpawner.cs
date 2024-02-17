using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class SpellSpawner : MonoBehaviour
{
    private PlayerController m_player;

    void OnEnable()
    {
        m_player = FindObjectOfType<PlayerController>();
    }

    public void SpawnBaseArcher(SO_ActiveSpells _data, ObjectPool<Spell_BaseArcher> _pool, Transform _parent)
    {
        for (int i = 0; i < _data.ProjectileAmount[_data.Level-1]; i++)
        {
            Spell_BaseArcher spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(m_player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_data);
        }
    }

    /// <summary>
    /// Spawn the Spell "AllDirections"
    /// </summary>
    public void SpawnAllDirections(SO_ActiveSpells _data, ObjectPool<Spell_AllDirections> _pool, Transform _parent)
    {
        for (int i = 0; i < _data.ProjectileAmount[_data.Level-1]; i++)
        {
            Spell_AllDirections spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(m_player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_data, i);
        }
    }

    /// <summary>
    /// Spawn the Spell "NearPlayer"
    /// </summary>
    public void SpawnNearPlayer(SO_ActiveSpells _data, ObjectPool<Spell_NearPlayer> _pool, Transform _parent)
    {
        for (int i = 0; i < _data.ProjectileAmount[_data.Level-1]; i++)
        {
            Spell_NearPlayer spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(m_player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_data);
        }
    }

    /// <summary>
    /// Spawn the Spell "NearPlayer"
    /// </summary>
    public void SpawnNewSpell1(SO_ActiveSpells _data, ObjectPool<Spell_NewSpell1> _pool, Transform _parent)
    {
        for (int i = 0; i < _data.ProjectileAmount[_data.Level - 1]; i++)
        {
            Spell_NewSpell1 spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(m_player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_data);
        }
    }
    
    /// <summary>
    /// Spawn the Spell "NearPlayer"
    /// </summary>
    public void SpawnNewSpell2(SO_ActiveSpells _data, ObjectPool<Spell_NewSpell2> _pool, Transform _parent)
    {
        for (int i = 0; i < _data.ProjectileAmount[_data.Level - 1]; i++)
        {
            Spell_NewSpell2 spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(m_player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_data, i);
        }
    }
    
    /// <summary>
    /// Spawn the Spell "NearPlayer"
    /// </summary>
    public void SpawnNewSpell3(SO_ActiveSpells _data, ObjectPool<Spell_NewSpell3> _pool, Transform _parent)
    {
        for (int i = 0; i < _data.ProjectileAmount[_data.Level - 1]; i++)
        {
            Spell_NewSpell3 spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(m_player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_data);
        }
    }
}
