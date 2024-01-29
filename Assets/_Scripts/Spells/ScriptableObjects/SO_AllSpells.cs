using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

[CreateAssetMenu(fileName = "AllSpellSO", menuName = "PixelPurge/Spells/AllSpellSO", order = 0)]
public class SO_AllSpells : ScriptableObject
{
    public SO_Spells[] spellSO;

    public SO_NearPlayer NearPlayer;
    public SO_BaseArcher BaseArcher;
    public SO_Aura Aura;
    public SO_AllDirections AllDirections;
}