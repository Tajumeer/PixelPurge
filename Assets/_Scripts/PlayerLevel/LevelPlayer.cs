using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPlayer : MonoBehaviour
{
    private CircleCollider2D col;

    private int level;

    /// <summary>
    /// the current amount of xp
    /// </summary>
    private int xpPoints;

    /// <summary>
    /// the amount of xp needed to level up
    /// </summary>
    [Tooltip("The amount of xp needed for the first level")]
    [SerializeField] private int xpPointsNeeded;

    [Tooltip("Increase the amount of XP needed to level up every Level by XX%")]
    [SerializeField] private float xpNeedMultiplier;

    private void Awake()
    {
        col = GetComponent<CircleCollider2D>();
    }

    /// <summary>
    /// Increase XP Points by 1
    /// </summary>
    public void GetXP()
    {
        xpPoints++;

        // if current xp = needed xp -> level up
        if(xpPoints >= xpPointsNeeded)  
        {
            LevelUp();

            xpPoints = 0;                                       // reset current xp
            xpPointsNeeded *= (int)(1f + xpNeedMultiplier);     // increase needed amount of xp
        }
    }

    public void IncreaseRadius(float _increase)
    {
        col.radius *= (1f + _increase);
    }

    private void LevelUp()
    {
        level++;

        // UI Selection
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (!_collision.gameObject.CompareTag("XP")) return;

        _collision.gameObject.TryGetComponent(out LevelXP character);
        character.CollectXP();
    }
}
