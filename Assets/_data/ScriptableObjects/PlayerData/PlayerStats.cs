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
    public float DamageMultiplier;
    public float CritChance;
    public float CritMultiplier;
    public float AttackSpeed;
    public float AreaMultiplier;
    public float ProjectileSpeed;
    public float RecastTimeMultiplier;

    [Header("Defensive Stats")]
    public float MaxHealth;
    public float HealthRegeneration;
    public float DamageReductionPercentage;

    [Header("Utility Stats")]
    public float CollectionRadius;
    public float GoldMultiplier;

    [Header("XP Stats")]
    public float XPNeeded;
    public float XPNeededMultiplier;
    public float XPMultiplier;
}
