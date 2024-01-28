using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class LevelPlayer : MonoBehaviour
{
    private CircleCollider2D col;
    private PlayerController playerData;

    [Header("Object Pool")]
    [SerializeField] private GameObject xpPrefab;
    private ObjectPool<LevelXP> xpPool;
    private GameObject xpParent;

    private int level;

    /// <summary>
    /// the current amount of xp
    /// </summary>
    private float xpPoints;

    /// <summary>
    /// the amount of xp needed to level up
    /// </summary>
    private float xpPointsNeeded;

    /// <summary>
    /// Increase the amount of XP needed to level up every Level by XX%
    /// </summary>
    private float xpNeedMultiplier;

    /// <summary>
    /// Set values from playerData and create ObjectPool
    /// </summary>
    public void InitXP()
    {
        col = GetComponent<CircleCollider2D>();
        playerData = GetComponentInParent<PlayerController>();

        // Set values from playerData
        level = 1;
        xpPointsNeeded = playerData.XPNeeded;
        xpNeedMultiplier = playerData.XPNeededMultiplier;
        IncreaseCollectionRadius(playerData.CollectionRadius);

        // Create Object Pool and Parent
        xpPool = new ObjectPool<LevelXP>(xpPrefab);
        xpParent = new GameObject();
        xpParent.name = "XP";

      //  StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        for(int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(1f);
            SpawnXP(new Vector3(Random.Range(-8, 8), Random.Range(-4, 4), 0f), 1f);
        }
    }

    /// <summary>
    /// Spawn an object from the xp Prefab
    /// </summary>
    /// <param name="_position">The position of the xp / where the enemy died</param>
    /// <param name="_xpAmount">The amount of xp points the player gets when collecting this xp object</param>
    public void SpawnXP(Vector3 _position, float _xpAmount)
    {
        LevelXP xp = xpPool.GetObject();

        xp.transform.SetParent(xpParent.transform);
        xp.ResetObj(_position, new Vector3(0f, 0f, 0f));

        xp.OnSpawn(_xpAmount, this);
    }

    /// <summary>
    /// Increase XP Points
    /// </summary>
    /// <param name="_amount">The amount of XP the player gets</param>
    public void GetXP(float _amount)
    {
        Debug.Log("Get XP: " + _amount);
        xpPoints += _amount * (1f + playerData.XPMultiplier);

        // if current xp = needed xp -> level up
        if (xpPoints >= xpPointsNeeded)
            LevelUp();
    }

    /// <summary>
    /// Increase the Radius of the Collection Trigger
    /// </summary>
    /// <param name="_radius"></param>
    public void IncreaseCollectionRadius(float _radius)
    {
        col.radius = _radius; // + 0.1f
    }

    private void LevelUp()
    {
        xpPoints = 0;                                   // reset current xp
        xpPointsNeeded *= (1f + xpNeedMultiplier);      // increase needed amount of xp

        level++;
        playerData.Level = level;

        FindObjectOfType<DungeonHUDManager>().LoadLevelUp();
    }
}
