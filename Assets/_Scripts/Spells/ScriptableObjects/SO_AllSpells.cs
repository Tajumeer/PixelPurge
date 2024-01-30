using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

[CreateAssetMenu(fileName = "AllSpellSO", menuName = "PixelPurge/Spells/AllSpellSO", order = 0)]
public class SO_AllSpells : ScriptableObject
{
    public SO_Spells[] spellSO;
}