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
        for (int i = 0; i < _data.ProjectileAmount[_data.Level - 1]; i++)
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
        for (int i = 0; i < _data.ProjectileAmount[_data.Level - 1]; i++)
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
        StartCoroutine(SpawnNearPlayerWithDelay(_data, _pool, _parent));
    }

    private IEnumerator SpawnNearPlayerWithDelay(SO_ActiveSpells _data, ObjectPool<Spell_NearPlayer> _pool, Transform _parent)
    {
        for (int i = 0; i < _data.ProjectileAmount[_data.Level - 1]; i++)
        {
            Spell_NearPlayer spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(m_player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_data);

            yield return new WaitForSeconds(_data.Cd[_data.Level - 1] / _data.ProjectileAmount[_data.Level - 1]);
        }
    }

    public void SpawnBoomerang(SO_ActiveSpells _data, ObjectPool<Spell_Boomerang> _pool, Transform _parent)
    {
        StartCoroutine(SpawnBoomerangWithDelay(_data, _pool, _parent));
    }

    private IEnumerator SpawnBoomerangWithDelay(SO_ActiveSpells _data, ObjectPool<Spell_Boomerang> _pool, Transform _parent)
    {
        for (int i = 0; i < _data.ProjectileAmount[_data.Level - 1]; i++)
        {
            Spell_Boomerang spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(m_player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_data, i);

            yield return new WaitForSeconds(_data.Cd[_data.Level - 1] / _data.ProjectileAmount[_data.Level - 1]);
        }
    }

    public void SpawnProtectiveOrbs(SO_ActiveSpells _data, ObjectPool<Spell_ProtectiveOrbs> _pool, Transform _parent)
    {
        for (int i = 0; i < _data.ProjectileAmount[_data.Level - 1]; i++)
        {
            Spell_ProtectiveOrbs spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(m_player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_data, i);
        }
    }

    public void SpawnGroundMine(SO_ActiveSpells _data, ObjectPool<Spell_GroundMine> _pool, Transform _parent)
    {
        for (int i = 0; i < _data.ProjectileAmount[_data.Level - 1]; i++)
        {
            Spell_GroundMine spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(m_player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_data);
        }
    }

    public void SpawnShockwave(SO_ActiveSpells _data, ObjectPool<Spell_Shockwave> _pool, Transform _parent)
    {
        Spell_Shockwave spellObj = _pool.GetObject();

        if (spellObj.tag != "PlayerSpell")
        {
            spellObj.transform.SetParent(_parent);
            spellObj.tag = "PlayerSpell";
        }

        spellObj.ResetObj(m_player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

        spellObj.OnSpawn(_data);
    }

    public void SpawnBomb(SO_ActiveSpells _data, ObjectPool<Spell_Bomb> _pool, Transform _parent)
    {
        for (int i = 0; i < _data.ProjectileAmount[_data.Level - 1]; i++)
        {
            Spell_Bomb spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(m_player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_data, m_player.gameObject.transform);
        }
    }

    public void SpawnPoisonArea(SO_ActiveSpells _data, ObjectPool<Spell_PoisonArea> _pool, Transform _parent)
    {
        for (int i = 0; i < _data.ProjectileAmount[_data.Level - 1]; i++)
        {
            Spell_PoisonArea spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(m_player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_data, m_player.gameObject.transform);
        }
    }

    public void SpawnChainLightning(SO_ActiveSpells _data, ObjectPool<Spell_ChainLightning> _pool, Transform _parent)
    {
        for (int i = 0; i < _data.ProjectileAmount[_data.Level - 1]; i++)
        {
            Spell_ChainLightning spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(m_player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_data);
        }
    }

    public void SpawnArrowVolley(SO_ActiveSpells _data, ObjectPool<Spell_ArrowVolley> _pool, Transform _parent)
    {
        for (int i = 0; i < _data.ProjectileAmount[_data.Level - 1]; i++)
        {
            Spell_ArrowVolley spellObj = _pool.GetObject();

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
