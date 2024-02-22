using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PixelPurge/PlayerData", order = 2)]
public class PlayerStats : ScriptableObject
{
    [Header("Class Data")]
    public string PlayerClassName;
    public int Level;
    public float CurrentHealth;
   

    [Header("Movement Stats")]
    public float MovementSpeed;
    public int DashAmount;
    public float DashSpeed;
    public float DashCooldown;
    public float DashTime;

    [Header("Offensive Stats")]
    [Tooltip("1.XX %")]
    public float DamageMultiplier;
    [Tooltip("0.XX %")]
    public float CritChance;
    [Tooltip("1.XX %")]
    public float CritMultiplier;
    [Tooltip("1.XX %")]
    public float AreaMultiplier;
    public float ProjectileSpeed;
    [Tooltip("0.XX % - 1 is no CD and 0 is whole CD")]
    public float CdReduction;

    [Header("Defensive Stats")]
    public float MaxHealth;
    public float HealthRegeneration;
    [Tooltip("0.XX %")]
    public float DamageReductionPercentage;

    [Header("Utility Stats")]
    public float CollectionRadius;
    [Tooltip("1.XX %")]
    public float GoldMultiplier;

    [Header("XP Stats")]
    public float XPNeeded;
    [Tooltip("0.XX %")]
    public float XPNeededMultiplier;
    [Tooltip("1.XX %")]
    public float XPMultiplier;
}
