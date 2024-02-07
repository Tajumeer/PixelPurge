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

    //Audio
    public float MasterVolume;
    public float MusicVolume;
    public float EffectVolume;

    //Graphics
    public int FullScreenMode;
    public int ResolutionValue;
  //public bool Toggled;

    public GameData()
    {
        this.MovementSpeedLevel = 0;
        this.CollectionRadiusLevel = 0;
        this.CriticalDamageLevel = 0;
        this.HealthLevel = 0;
        this.AttackSpeedLevel = 0;

        this.MasterVolume = 0;
        this.EffectVolume = 0;
        this.MusicVolume = 0;

        this.FullScreenMode = 3;
        this.ResolutionValue = 2;
        //this.Toggled = false;
    }
}
