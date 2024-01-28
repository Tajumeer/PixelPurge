using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

[CreateAssetMenu(fileName = "AllSpellSO", menuName = "PixelPurge/Spells/AllSpellSO", order = 1)]
public class SO_AllSpellSO : ScriptableObject
{
    public SO_BaseArcher BaseArcher;
    public SO_AllDirections AllDirections;
    public SO_NearPlayer NearPlayer;
    public SO_Aura Aura;
}