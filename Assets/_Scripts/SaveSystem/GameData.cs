using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //stats to save here
    public int MovementSpeedLevel;
    public int CollectionRadiusLevel;
    public int CriticalDamageLevel;
    public int HealthLevel;
    public int AttackSpeedLevel;

    public GameData()
    {
        this.MovementSpeedLevel = 0;
        this.CollectionRadiusLevel = 0;
        this.CriticalDamageLevel = 0;
        this.HealthLevel = 0;
        this.AttackSpeedLevel = 0;
    }
}
