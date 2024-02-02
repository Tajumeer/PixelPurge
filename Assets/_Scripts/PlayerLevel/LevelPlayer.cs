using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class LevelPlayer : MonoBehaviour
{
    private CircleCollider2D m_col;
    private PlayerController m_playerData;
    private DungeonHUDManager m_hudManager;

    [Header("Object Pool")]
    [SerializeField] private GameObject m_xpPrefab;
    private ObjectPool<LevelXP> m_xpPool;
    private GameObject m_xpParent;

    private int m_level;

    /// <summary>
    /// the current amount of xp
    /// </summary>
    private float m_xpPoints;

    /// <summary>
    /// the amount of xp needed to level up
    /// </summary>
    private float m_xpPointsNeeded;

    /// <summary>
    /// Increase the amount of XP needed to level up every Level by XX%
    /// </summary>
    private float m_xpNeedMultiplier;

    IEnumerator SetXP()
    {
        yield return new WaitForSeconds(0.5f);
        ChangeXPValue(0);
    }

    /// <summary>
    /// Set values from playerData and create ObjectPool
    /// </summary>
    public void InitXP()
    {
        m_col = GetComponent<CircleCollider2D>();
        m_playerData = GetComponentInParent<PlayerController>();

        // Set values from playerData
        m_level = 1;
        m_xpPointsNeeded = m_playerData.XPNeeded;
        StartCoroutine(SetXP());
        m_xpNeedMultiplier = m_playerData.XPNeededMultiplier;
        IncreaseCollectionRadius(m_playerData.CollectionRadius);

        // Create Object Pool and Parent
        m_xpPool = new ObjectPool<LevelXP>(m_xpPrefab);
        m_xpParent = new GameObject();
        m_xpParent.name = "XP";

        //  StartCoroutine(Spawn());
    }

    /// <summary>
    /// Spawn an object from the xp Prefab
    /// </summary>
    /// <param name="_position">The position of the xp / where the enemy died</param>
    /// <param name="_xpAmount">The amount of xp points the player gets when collecting this xp object</param>
    public void SpawnXP(Vector3 _position, float _xpAmount)
    {
        LevelXP xp = m_xpPool.GetObject();

        xp.transform.SetParent(m_xpParent.transform);
        xp.ResetObj(_position, new Vector3(0f, 0f, 0f));

        xp.OnSpawn(_xpAmount, this);
    }

    /// <summary>
    /// Increase XP Points
    /// </summary>
    /// <param name="_amount">The amount of XP the player gets</param>
    public void GetXP(float _amount)
    {
        ChangeXPValue(m_xpPoints + _amount * (1f + m_playerData.XPMultiplier));

        // if current xp = needed xp -> level up
        if (m_xpPoints >= m_xpPointsNeeded)
            LevelUp();
    }

    /// <summary>
    /// Increase the Radius of the Collection Trigger
    /// </summary>
    /// <param name="_radius"></param>
    public void IncreaseCollectionRadius(float _radius)
    {
        m_col.radius = _radius; // + 0.1f
    }

    private void LevelUp()
    {
        ChangeXPValue(0);                                   // reset current xp
        m_xpPointsNeeded *= (1f + m_xpNeedMultiplier);      // increase needed amount of xp

        m_level++;
        m_playerData.Level = m_level;

        if (m_hudManager == null)
        {
            if (m_hudManager = FindObjectOfType<DungeonHUDManager>())
                m_hudManager.LoadLevelUp();
            else Debug.LogWarning(m_hudManager.GetType() + " not found");
        }
        else m_hudManager.LoadLevelUp();
    }

    private void ChangeXPValue(float _value)
    {
        m_xpPoints = _value;

        if (m_hudManager == null)
        {
            if (m_hudManager = FindObjectOfType<DungeonHUDManager>())
                m_hudManager.ShowXP(m_xpPoints, m_xpPointsNeeded);
            else Debug.LogWarning(m_hudManager.GetType() + " not found");
        }
        else m_hudManager.ShowXP(m_xpPoints, m_xpPointsNeeded);
    }
}
