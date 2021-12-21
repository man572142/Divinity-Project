using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Skill")]
public class SkillSet : ScriptableObject
{
    Occupation myOccupation;
    Team myTeam;

    public int myID;
    public Sprite icon;
    public string skillName;
    

    public float attackCoefficient;
    public float skillRangeCoefficient;
    [HideInInspector] public float skillDamage;
    //public int healingPoint;
    public int actionPoint;
    public int coolDown;
    

}
