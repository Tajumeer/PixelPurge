using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveData", menuName = "PixelPurge/PassiveData", order = 3)]
public class SO_PassiveData : ScriptableObject
{
    [Header("Movement Stats")]
    public int MovementSpeed_Level;
    public float[] MovementSpeed;

    [Header("Offensive Stats")]
    public float[] DamageMultiplier;
    public float[] CritChance;
    public float[] CritMultiplier;
    public float[] AttackSpeed;
    public float[] AreaMultiplier;
    public float[] RecastTimeMultiplier;

    [Header("Defensive Stats")]
    public int MaxHealth_Level;
    public float[] MaxHealth;
    public float[] HealthRegeneration;
    public float[] DamageReductionPercentage;

    [Header("Utility Stats")]
    public float[] CollectionRadius;
    public float[] XPMultiplier;
}
