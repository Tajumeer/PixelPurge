using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Maya

public class LevelUpManager : MonoBehaviour
{
    [SerializeField] private SO_AllSpells m_dataSpells;

    [SerializeField] private GameObject m_prefab_spellCardActive;
    [SerializeField] private GameObject m_prefab_spellCardPassive;
    [SerializeField] private GameObject m_prefab_spellCardGold;
    [SerializeField] private Transform m_spellCardParent;

    [Tooltip("How many Active / passive spells can be learned in one run")]
    [SerializeField] private int m_spellSlots = 4;

    private void OnEnable()
    {
        RandomizeSpellsToShow(GetAvailableSpells());
    }

    #region GetAvailableSpells

    /// <summary>
    /// Makes a List of all available Spells to learn/upgrade 
    /// (dont show new active or passive spells when there are no free slots)
    /// </summary>
    private List<Spells> GetAvailableSpells()
    {
        List<Spells> availableSpells = new List<Spells>();

        // lists of all active/passive spells that can be learned or upgraded
        List<Spells> availableActiveSpells = GetAvailableActiveSpells();
        // dont list passives if its the first "levelup", because then we only want actives 
        List<Spells> availablePassiveSpells = new List<Spells>(); ;
        if (m_dataSpells.statSO.Level > 1) availablePassiveSpells = GetAvailablePassiveSpells();

        // combine the lists of active and passive spells

        if (availableActiveSpells.Count > 0)    // safety check
        {
            foreach (Spells spell in availableActiveSpells)
            {
                availableSpells.Add(spell);
            }
        }

        if (availablePassiveSpells.Count > 0)   // safety check
        {
            foreach (Spells spell in availablePassiveSpells)
            {
                availableSpells.Add(spell);
            }
        }
        return availableSpells;
    }

    /// <summary>
    /// Go through every activeSpell and add it to the list if its not on max level
    /// </summary>
    /// <returns>List of all available Active Spells || true: active spell slots are not full</returns>
    private List<Spells> GetAvailableActiveSpells()
    {
        List<Spells> availableActiveSpells = new List<Spells>();
        List<Spells> availableNewActiveSpells = new List<Spells>();
        List<Spells> availableOldActiveSpells = new List<Spells>();

        int learnedActiveSpellAmount = 0;

        for (int i = 0; i < (int)Spells.ActiveSpells; i++)
        {
            // if at this place is no spell skip
            if (m_dataSpells.activeSpellSO[i] == null) continue;

            // if spell is already at max level, its not available for choosing
            if (m_dataSpells.activeSpellSO[i].Level >= m_dataSpells.activeSpellSO[i].MaxLevel)
            {
                learnedActiveSpellAmount++;
                continue;
            }

            // put the spell in the list of the learned or new ones
            if (m_dataSpells.activeSpellSO[i].Level > 0)
            {
                learnedActiveSpellAmount++;
                availableOldActiveSpells.Add((Spells)i);
            }
            else
                availableNewActiveSpells.Add((Spells)i);
        }

        // if we can learn new Spells and the list of old spells is not full, add them to the list
        if (availableNewActiveSpells.Count > 0 && learnedActiveSpellAmount < m_spellSlots)
        {
            foreach (Spells spell in availableNewActiveSpells)
            {
                availableActiveSpells.Add(spell);
            }
        }

        if (availableOldActiveSpells.Count > 0)     // safety check
        {
            foreach (Spells spell in availableOldActiveSpells)
            {
                availableActiveSpells.Add(spell);
            }
        }
        // add all spells that can be upgraded

        return availableActiveSpells;
    }

    /// <summary>
    /// Go through every passive Spell and add it to the list if its not on max level
    /// </summary>
    /// <returns>List of all available Passive Spells || true: passive spell slots are not full</returns>
    private List<Spells> GetAvailablePassiveSpells()
    {
        List<Spells> availablePassiveSpells = new List<Spells>();
        List<Spells> availableNewPassiveSpells = new List<Spells>();
        List<Spells> availableOldPassiveSpells = new List<Spells>();

        int learnedPassiveSpellAmount = 0;

        // go through all passive Spells
        for (int i = (int)Spells.ActiveSpells + 1; i < (int)Spells.PassiveSpells; i++)
        {
            // get the index of the passive spell ("delete" active Spells for index)
            int idx = i - ((int)Spells.ActiveSpells + 1);

            // if at this place is no spell skip
            if (m_dataSpells.passiveSpellSO[idx] == null) continue;

            // if spell is already at max level, its not available for choosing
            if (m_dataSpells.passiveSpellSO[idx].Level >= m_dataSpells.passiveSpellSO[idx].MaxLevel)
            {
                learnedPassiveSpellAmount++;
                continue;
            }

            // put the spell in the list of the learned or new ones
            if (m_dataSpells.passiveSpellSO[idx].Level > 0)
            {
                learnedPassiveSpellAmount++;
                availableOldPassiveSpells.Add((Spells)i);
            }
            else
                availableNewPassiveSpells.Add((Spells)i);
        }

        // if we can learn new Spells and the list of old spells is not full, add them to the list
        if (availableNewPassiveSpells.Count > 0 && learnedPassiveSpellAmount < m_spellSlots)
        {
            foreach (Spells spell in availableNewPassiveSpells)
            {
                availablePassiveSpells.Add(spell);
            }
        }

        // add all spells that can be upgraded
        if (availableOldPassiveSpells.Count > 0)    // safety check
        {
            foreach (Spells spell in availableOldPassiveSpells)
            {
                availablePassiveSpells.Add(spell);
            }
        }

        return availablePassiveSpells;
    }

    #endregion

    /// <summary>
    /// Random choose 3 Spells from the List and show them in the UI
    /// </summary>
    /// <param name="_availableSpells"></param>
    private void RandomizeSpellsToShow(List<Spells> _availableSpells)
    {
        // choose 3 spells from List
        for (int i = 0; i < 3; i++)
        {
            // if there are no spell (upgrades) left, make a gold card
            if (_availableSpells.Count == 0)
            {
                CardGold();
                continue;
            }

            // choose a random Spell from all available Spells List
            int randomSpell = Random.Range(0, _availableSpells.Count);

            Spells chosenSpell = _availableSpells[randomSpell];

            // and put it in the HUD (check if its an active or passive spell)
            if ((int)chosenSpell < (int)Spells.ActiveSpells)
                CardActiveSpell(chosenSpell);
            else if ((int)chosenSpell > (int)Spells.ActiveSpells && (int)chosenSpell < (int)Spells.PassiveSpells)
                CardPassiveSpell(chosenSpell);

            // Remove this spell from list to avoid showing it multiple times
            _availableSpells.RemoveAt(randomSpell);  // remove spell from list because its already in the HUD
        }
    }

    #region Cards

    /// <summary>
    /// Creates a Active Spell Card from the Prefab and set the values to the specific spell (icon, name, description, etc.)
    /// </summary>
    /// <param name="_spell"></param>
    private void CardActiveSpell(Spells _spell)
    {
        // get the Scriptable Object of this spell
        SO_ActiveSpells spellSO = m_dataSpells.activeSpellSO[(int)_spell];

        // create new GameObject
        GameObject spellCard = Instantiate(m_prefab_spellCardActive);

        spellCard.transform.SetParent(m_spellCardParent);

        // set spell, icon, name
        ChooseSpell values = spellCard.GetComponent<ChooseSpell>();
        values.m_spell = _spell;
        values.m_icon.sprite = spellSO.SpellIcon;
        values.m_name.text = spellSO.SpellName;
        if (spellSO.Level == 0) values.m_newText.SetActive(true);
        else values.m_newText.SetActive(false);

        // if its not already learned, show spell description
        if (spellSO.Level == 0 && spellSO.MaxLevel != 0)
        {
            values.m_description.alignment = TMPro.TextAlignmentOptions.Center;
            values.m_description.text = spellSO.SpellDescription;
        }
        // else show upgrades
        else
        {
            values.m_description.text = PrintActiveUpgrades(spellSO);
        }
    }

    /// <summary>
    /// Creates a Passive Spell Card from the Prefab and set the values to the specific spell (icon, name, description, etc.)
    /// </summary>
    /// <param name="_spell"></param>
    private void CardPassiveSpell(Spells _spell)
    {
        // get the index of the passive spell ("delete" active Spells for index)
        int idx = (int)_spell - ((int)Spells.ActiveSpells + 1);

        // get the Scriptable Object of this spell
        SO_PassiveSpells spellSO = m_dataSpells.passiveSpellSO[idx];

        // create new GameObject
        GameObject spellCard = Instantiate(m_prefab_spellCardPassive);
        spellCard.transform.SetParent(m_spellCardParent);

        // set spell, icon, name
        ChooseSpell values = spellCard.GetComponent<ChooseSpell>();
        values.m_spell = _spell;
        values.m_icon.sprite = spellSO.SpellIcon;
        values.m_name.text = spellSO.SpellName;
        if (spellSO.Level == 0) values.m_newText.SetActive(true);
        else values.m_newText.SetActive(false);

        //values.m_description.text = GetSpellDescription(spellSO.Stat[spellSO.Level], spellSO.Stat[spellSO.Level + 1], spellSO.SpellUpgradeDescription);

        // Show upgrade in the spell description
        (float upgrade, bool percent) = CheckUpgrade(spellSO.Stat[spellSO.Level]);

        if (upgrade == 0f) values.m_description.text = "Hidden Upgrade :)";  // safety check 

        if (percent)
            values.m_description.text = "+ " + upgrade + "% " + spellSO.SpellUpgradeDescription;
        else
            values.m_description.text = "+ " + upgrade + " " + spellSO.SpellUpgradeDescription;
    }

    /// <summary>
    /// Create a Gold Card from the Prefab
    /// </summary>
    private void CardGold()
    {
        // create new GameObject
        GameObject spellCard = Instantiate(m_prefab_spellCardGold);
        spellCard.transform.SetParent(m_spellCardParent);
    }

    #endregion

    #region Upgrades and Descriptions

    private string PrintActiveUpgrades(SO_ActiveSpells _spellSO)
    {
        string description = "";

        // Damage
        if (_spellSO.Damage.Length == _spellSO.MaxLevel)
            description +=
                GetSpellDescription(_spellSO.Damage[_spellSO.Level - 1], _spellSO.Damage[_spellSO.Level], m_dataSpells.nameSpellSO.Damage);

        // Lifetime
        if (_spellSO.Lifetime.Length == _spellSO.MaxLevel)
            description +=
            GetSpellDescription(_spellSO.Lifetime[_spellSO.Level - 1], _spellSO.Lifetime[_spellSO.Level], m_dataSpells.nameSpellSO.Lifetime);

        // Speed
        if (_spellSO.Speed.Length == _spellSO.MaxLevel)
            description +=
            GetSpellDescription(_spellSO.Speed[_spellSO.Level - 1], _spellSO.Speed[_spellSO.Level], m_dataSpells.nameSpellSO.Speed);

        // Cd
        if (_spellSO.Cd.Length == _spellSO.MaxLevel)
            description +=
            GetSpellDescription(_spellSO.Cd[_spellSO.Level - 1], _spellSO.Cd[_spellSO.Level], m_dataSpells.nameSpellSO.Cd);

        // InternalCd
        if (_spellSO.InternalCd.Length == _spellSO.MaxLevel)
            description +=
            GetSpellDescription(_spellSO.InternalCd[_spellSO.Level - 1], _spellSO.InternalCd[_spellSO.Level], m_dataSpells.nameSpellSO.InternalCd);

        // Radius
        if (_spellSO.Radius.Length == _spellSO.MaxLevel)
            description +=
            GetSpellDescription(_spellSO.Radius[_spellSO.Level - 1], _spellSO.Radius[_spellSO.Level], m_dataSpells.nameSpellSO.Radius);

        // ProjectileAmount
        if (_spellSO.ProjectileAmount.Length == _spellSO.MaxLevel)
            description +=
            GetSpellDescription(_spellSO.ProjectileAmount[_spellSO.Level - 1], _spellSO.ProjectileAmount[_spellSO.Level],
                                m_dataSpells.nameSpellSO.ProjectileAmount);

        // Bounce
        if (_spellSO.Bounce.Length == _spellSO.MaxLevel)
            description +=
            GetSpellDescription(_spellSO.Bounce[_spellSO.Level - 1], _spellSO.Bounce[_spellSO.Level], m_dataSpells.nameSpellSO.Bounce);

        // Pierce
        if (_spellSO.Pierce.Length == _spellSO.MaxLevel)
            description +=
            GetSpellDescription(_spellSO.Pierce[_spellSO.Level - 1], _spellSO.Pierce[_spellSO.Level], m_dataSpells.nameSpellSO.Pierce);

        return description;
    }

    private string GetSpellDescription(float _activeLevel, float nextLevel, string statName)
    {
        string description = "";

        // check if this stat would be upgraded on the next level
        if (_activeLevel != nextLevel)
        {
            // check what kind of upgrade it is
            (float upgrade, bool percent) = CheckUpgrade(nextLevel);

            if (upgrade == 0f) description += "Hidden Upgrade :)";  // safety check 

            // and set description
            if (percent)
                description += "+ " + upgrade + "% " + statName + "\n";
            else
            {
                float subtraction = upgrade - _activeLevel;
                if (subtraction < 0) subtraction *= -1f;    // e.g. for cd reduction the upgrade would be -1
                description += "+ " + subtraction + " " + statName + "\n";
            }
        }

        return description;
    }

    /// <summary>
    /// Checks if the Upgrade is a percent value or a whole number 
    /// </summary>
    /// <param name="_stat"></param>
    /// <returns>float: the upgrade || bool: true if its a percent value</returns>
    private (float, bool) CheckUpgrade(float _stat)
    {
        float upgrade = 0f;
        bool percent = false; ;

        // if Stat is a positive integer -> is is no % but its value is added (e.g. 20)
        if (_stat >= 1 && _stat % 1 == 0)
        {
            upgrade = _stat;
            percent = false;
        }
        // if Stat is greater than 1 but is a decimal number -> it is a % value (e.g. 1.4)
        else if (_stat > 1 && _stat % 1 != 0)
        {
            upgrade = (_stat - 1) * 100;
            percent = true;
        }
        // if Stat is between 0 and 1 -> it is a % value (e.g. 0.6)
        else if (_stat < 1 && _stat > 0)
        {
            upgrade = (1 - _stat) * 100;
            percent = true;
        }

        return (upgrade, percent);
    }

    #endregion
}
