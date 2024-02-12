using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //stats to save here
    public int HealthLevel;
    public int HealthRegenLevel;
    public int DamageLevel;
    public int CriticalChanceLevel;
    public int CollectionRadiusLevel;
    public int MovementSpeedLevel;
    public int GoldLevel;
    public int XPLevel;

    //UIData Variables
    //Audio
    public float MasterVolume;
    public float MusicVolume;
    public float EffectVolume;

    //Graphics
    public int FullScreenMode;
    public int ResolutionValue;
    //public bool Toggled;

    //Shop Variables
    public int[] StatLevel;

    //GameManager Variables
    //Leaderboard 
    public string UserName;
    public int HighScore;

    //Currency
    public int Gold;

    public GameData()
    {
        this.HealthLevel = 0;
        this.HealthRegenLevel = 0;
        this.DamageLevel = 0;
        this.CriticalChanceLevel = 0;
        this.CollectionRadiusLevel = 0;
        this.MovementSpeedLevel = 0;
        this.GoldLevel = 0;
        this.XPLevel = 0;

        this.MasterVolume = 0;
        this.EffectVolume = 0;
        this.MusicVolume = 0;

        this.FullScreenMode = 3;
        this.ResolutionValue = 2;
        //this.Toggled = false;

        this.UserName = "";
        this.HighScore = 0;

        this.Gold = 0;

        this.StatLevel = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
    }
}
