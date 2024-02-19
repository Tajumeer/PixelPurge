using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

[CreateAssetMenu(fileName = "OriginalSpells", menuName = "PixelPurge/Spells/OriginalSpells", order = 1)]
public class SO_OriginalSpells : ScriptableObject
{
    [Header("Original Active Spell ScriptableObjects")]
    public SO_ActiveSpells Data_BaseArcher;
    public SO_ActiveSpells Data_AllDirections;
    public SO_ActiveSpells Data_NearPlayer;
    public SO_ActiveSpells Data_Aura;
    public SO_ActiveSpells Data_Boomerang;
    public SO_ActiveSpells Data_ProtectiveOrbs;
    public SO_ActiveSpells Data_GroundMine;

    [Header("Original Passive Spell ScriptableObjects")]
    public SO_PassiveSpells Data_MovementSpeed;
    public SO_PassiveSpells Data_DamageMultiplier;
    public SO_PassiveSpells Data_CritChance;
    public SO_PassiveSpells Data_CritMultiplier;
    public SO_PassiveSpells Data_AttackSpeed;
    public SO_PassiveSpells Data_AreaMultiplier;
    public SO_PassiveSpells Data_RecastTimeMultiplier;
    public SO_PassiveSpells Data_MaxHealth;
    public SO_PassiveSpells Data_HealthRegeneration;
    public SO_PassiveSpells Data_DamageReductionPercentage;
    public SO_PassiveSpells Data_CollectionRadius;
    public SO_PassiveSpells Data_XPMultiplier;
}
