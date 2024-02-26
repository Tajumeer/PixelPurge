using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

[CreateAssetMenu(fileName = "OriginalSpells", menuName = "PixelPurge/Spells/OriginalSpells", order = 1)]
public class SO_OriginalSpells : ScriptableObject
{
    [Header("Original Active Spell ScriptableObjects")]
    public SO_ActiveSpells Data_AirWave;
    public SO_ActiveSpells Data_ShurikenToss;
    public SO_ActiveSpells Data_HomingRock;
    public SO_ActiveSpells Data_Aura;
    public SO_ActiveSpells Data_Boomerang;
    public SO_ActiveSpells Data_SwordVortex;
    public SO_ActiveSpells Data_GroundMine;
    public SO_ActiveSpells Data_Shockwave;
    public SO_ActiveSpells Data_Bomb;
    public SO_ActiveSpells Data_PoisonArea;
    public SO_ActiveSpells Data_ToxicTrail;
    public SO_ActiveSpells Data_Shield;

    [Header("Original Passive Spell ScriptableObjects")]
    public SO_PassiveSpells Data_MovementSpeed;
    public SO_PassiveSpells Data_DamageMultiplier;
    public SO_PassiveSpells Data_CritChance;
    public SO_PassiveSpells Data_CritMultiplier;
    public SO_PassiveSpells Data_AreaMultiplier;
    public SO_PassiveSpells Data_CdReduction;
    public SO_PassiveSpells Data_MaxHealth;
    public SO_PassiveSpells Data_HealthRegeneration;
    public SO_PassiveSpells Data_DamageReductionPercentage;
    public SO_PassiveSpells Data_CollectionRadius;
    public SO_PassiveSpells Data_XPMultiplier;
}
