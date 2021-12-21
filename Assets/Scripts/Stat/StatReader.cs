using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatReader : MonoBehaviour
{

    [SerializeField] Dropdown occupationDropdown = null;
    public Dropdown OccupationDropDown { get { return occupationDropdown; } }
    [SerializeField] Dropdown healthDropdown = null;
    public Dropdown HealthDropDown {get{ return healthDropdown; }}
    [SerializeField] Dropdown abilityDropdown = null;
    public Dropdown AbilityDropDown { get { return abilityDropdown; } }
    [SerializeField] Team team = default;
    [SerializeField][Range(1,4)] int characterIndex = 0;
    int characterID = 0;
    [SerializeField] StatChecker statChecker = null;
    [SerializeField] BaseStat stat = null;


    Occupation[] occupationOption = {Occupation.None, Occupation.Warrior,Occupation.Archer,Occupation.Healer};

    public void SetDropdownValue()
    {
        statChecker.StatCheck();
    }

    private void Start()
    {
        CheckID();
        ReadDefault();
    }

    private void ReadDefault()
    {
        OccupationDropDown.value = stat.defaultStats[characterID].occupationIndex;
        healthDropdown.value = stat.defaultStats[characterID].healthIndex;
        abilityDropdown.value = stat.defaultStats[characterID].abilityIndex;
    }

    private void CheckID()
    {
        if (characterIndex == 0) return;

        if (team == Team.Player1)
        {
            characterID = characterIndex * 1 - 1;
        }
        else if (team == Team.Player2)
        {
            characterID = characterIndex * 2 - 1;
        }
    }

    private void OnDisable()
    {
        GameManager.instance.characterStats[characterID].occupation = occupationOption[occupationDropdown.value];

        GameManager.instance.characterStats[characterID].health = CalculateHealth();
        GameManager.instance.characterStats[characterID].ability = CalculateAbility();
    }

    private float CalculateHealth()
    {
        switch (healthDropdown.value)
        {                
            case 1:
                return stat.highRank * stat.baseHealth;
            case 2:
                return stat.mediumRank * stat.baseHealth;
            case 3:
                return stat.lowRank * stat.baseHealth;
        }
        Debug.LogError("Stat has not been set correctly");
        return 0;
    }

    private float CalculateAbility()
    {
        if(occupationDropdown.value == 3 )  //是補師
        {
            switch (abilityDropdown.value)
            {
                case 1:
                    return stat.highRank * stat.baseAbility;
                case 2:
                    return stat.mediumRank * stat.baseAbility;
                case 3:
                    return stat.lowRank * stat.baseAbility;
            }
            Debug.LogError("Stat has not been set correctly");
            return 0;
        }
        else  //是戰士或弓箭手
        {
            switch (abilityDropdown.value)
            {
                case 1:
                    return stat.highRank * stat.baseDamage;
                case 2:
                    return stat.mediumRank * stat.baseDamage;
                case 3:
                    return stat.lowRank * stat.baseDamage;
            }
            Debug.LogError("Stat has not been set correctly");
            return 0;
        }

        
    }

}
