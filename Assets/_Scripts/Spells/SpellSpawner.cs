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

    public void SpawnAirWave(PlayerStats _playerData, SO_ActiveSpells _spellData, ObjectPool<Spell_AirWave> _pool, Transform _parent)
    {
        for (int i = 0; i < _spellData.ProjectileAmount[_spellData.Level - 1]; i++)
        {
            Spell_AirWave spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(m_player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_playerData, _spellData);
        }
    }

    public void SpawnShurikenToss(PlayerStats _playerData, SO_ActiveSpells _spellData, ObjectPool<Spell_ShurikenToss> _pool, Transform _parent)
    {
        for (int i = 0; i < _spellData.ProjectileAmount[_spellData.Level - 1]; i++)
        {
            Spell_ShurikenToss spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(m_player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_playerData, _spellData, i);
        }
    }

    public void SpawnHomingRock(PlayerStats _playerData, SO_ActiveSpells _spellData, ObjectPool<Spell_HomingRock> _pool, Transform _parent)
    {
        StartCoroutine(SpawnNearPlayerWithDelay(_playerData, _spellData, _pool, _parent));
    }

    private IEnumerator SpawnNearPlayerWithDelay(PlayerStats _playerData, SO_ActiveSpells _spellData, ObjectPool<Spell_HomingRock> _pool, Transform _parent)
    {
        for (int i = 0; i < _spellData.ProjectileAmount[_spellData.Level - 1]; i++)
        {
            Spell_HomingRock spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(m_player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_playerData, _spellData);

            yield return new WaitForSeconds(_spellData.Cd[_spellData.Level - 1] / _spellData.ProjectileAmount[_spellData.Level - 1]);
        }
    }

    public void SpawnBoomerang(PlayerStats _playerData, SO_ActiveSpells _spellData, ObjectPool<Spell_Boomerang> _pool, Transform _parent)
    {
        StartCoroutine(SpawnBoomerangWithDelay(_playerData, _spellData, _pool, _parent));
    }

    private IEnumerator SpawnBoomerangWithDelay(PlayerStats _playerData, SO_ActiveSpells _spellData, ObjectPool<Spell_Boomerang> _pool, Transform _parent)
    {
        for (int i = 0; i < _spellData.ProjectileAmount[_spellData.Level - 1]; i++)
        {
            Spell_Boomerang spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(m_player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_playerData, _spellData, i);

            yield return new WaitForSeconds(_spellData.Cd[_spellData.Level - 1] / _spellData.ProjectileAmount[_spellData.Level - 1]);
        }
    }

    public void SpawnSwordVortex(PlayerStats _playerData, SO_ActiveSpells _spellData, ObjectPool<Spell_SwordVortex> _pool, Transform _parent)
    {
        for (int i = 0; i < _spellData.ProjectileAmount[_spellData.Level - 1]; i++)
        {
            Spell_SwordVortex spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(m_player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_playerData, _spellData, i);
        }
    }

    public void SpawnGroundMine(PlayerStats _playerData, SO_ActiveSpells _spellData, ObjectPool<Spell_GroundMine> _pool, Transform _parent)
    {
        for (int i = 0; i < _spellData.ProjectileAmount[_spellData.Level - 1]; i++)
        {
            Spell_GroundMine spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(m_player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_playerData, _spellData);
        }
    }

    public void SpawnShockwave(PlayerStats _playerData, SO_ActiveSpells _spellData, ObjectPool<Spell_Shockwave> _pool, Transform _parent)
    {
        Spell_Shockwave spellObj = _pool.GetObject();

        if (spellObj.tag != "PlayerSpell")
        {
            spellObj.transform.SetParent(_parent);
            spellObj.tag = "PlayerSpell";
        }

        spellObj.ResetObj(m_player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

        spellObj.OnSpawn(_playerData, _spellData);
    }

    public void SpawnBomb(PlayerStats _playerData, SO_ActiveSpells _spellData, ObjectPool<Spell_Bomb> _pool, Transform _parent)
    {
        for (int i = 0; i < _spellData.ProjectileAmount[_spellData.Level - 1]; i++)
        {
            Spell_Bomb spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(m_player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_playerData, _spellData, m_player.gameObject.transform);
        }
    }

    public void SpawnPoisonArea(PlayerStats _playerData, SO_ActiveSpells _spellData, ObjectPool<Spell_PoisonArea> _pool, Transform _parent)
    {
        for (int i = 0; i < _spellData.ProjectileAmount[_spellData.Level - 1]; i++)
        {
            Spell_PoisonArea spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(m_player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_playerData, _spellData, m_player.gameObject.transform);
        }
    }

    public void SpawnToxicTrail(PlayerStats _playerData, SO_ActiveSpells _spellData, ObjectPool<Spell_ToxicTrail> _pool, Transform _parent)
    {
        for (int i = 0; i < _spellData.ProjectileAmount[_spellData.Level - 1]; i++)
        {
            Spell_ToxicTrail spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(m_player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_playerData, _spellData, i);
        }
    }

    public void SpawnArrowVolley(PlayerStats _playerData, SO_ActiveSpells _spellData, ObjectPool<Spell_Boomerang> _pool, Transform _parent)
    {
        StartCoroutine(SpawnArrowVolleyDelay(_playerData, _spellData, _pool, _parent));
    }

    private IEnumerator SpawnArrowVolleyDelay(PlayerStats _playerData, SO_ActiveSpells _spellData, ObjectPool<Spell_Boomerang> _pool, Transform _parent)
    {
        for (int i = 0; i < _spellData.ProjectileAmount[_spellData.Level - 1]; i++)
        {
            Spell_Boomerang spellObj = _pool.GetObject();

            if (spellObj.tag != "PlayerSpell")
            {
                spellObj.transform.SetParent(_parent);
                spellObj.tag = "PlayerSpell";
            }

            spellObj.ResetObj(m_player.gameObject.transform.position, new Vector3(0f, 0f, 0f));

            spellObj.OnSpawn(_playerData, _spellData, i);

            yield return new WaitForSeconds(_spellData.Cd[_spellData.Level - 1] / _spellData.ProjectileAmount[_spellData.Level - 1]);
        }
    }
}
