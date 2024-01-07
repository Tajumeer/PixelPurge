using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class LevelPlayer : MonoBehaviour
{
    private CircleCollider2D col;
    private PlayerStats playerData;

    private int level;

    /// <summary>
    /// the current amount of xp
    /// </summary>
    private float xpPoints;

    /// <summary>
    /// the amount of xp needed to level up
    /// </summary>
    [Tooltip("The amount of xp needed for the first level")]
    [SerializeField] private float xpPointsNeeded;

    [Tooltip("Increase the amount of XP needed to level up every Level by XX%")]
    [SerializeField] private float xpNeedMultiplier;

    private void Start()
    {
        //xpPointsNeeded = playerData.XPNeeded;
        //xpNeedMultiplier = playerData.XPMultiplier; // !!!! XPNeededMultiplier 
    }

    public void initxp()
    {
        col = GetComponent<CircleCollider2D>();
        playerData = GetComponentInParent<PlayerController>().ActivePlayerData;

        level = 1;
        xpPointsNeeded = playerData.XPNeeded;
        xpNeedMultiplier = playerData.XPMultiplier; // !!!! XPNeededMultiplier 
        IncreaseRadius(playerData.CollectionRadius);
    }

    /// <summary>
    /// Increase XP Points
    /// </summary>
    /// <param name="_amount">The amount of XP the player gets</param>
    public void GetXP(float _amount)
    {
        Debug.Log("Get XP");
        xpPoints += _amount * (1f + playerData.XPMultiplier);

        // if current xp = needed xp -> level up
        if(xpPoints >= xpPointsNeeded)  
        {
            LevelUp();

            xpPoints = 0;                                       // reset current xp
            xpPointsNeeded *= (1f + xpNeedMultiplier);     // increase needed amount of xp
        }
    }

    /// <summary>
    /// Increase the Radius of the Collection Trigger
    /// </summary>
    /// <param name="_radius"></param>
    public void IncreaseRadius(float _radius)
    {
        col.radius = _radius; // + 0.1f
    }

    private void LevelUp()
    {
        level++;
        playerData.Level = level;

        // UI Selection
    }
}
