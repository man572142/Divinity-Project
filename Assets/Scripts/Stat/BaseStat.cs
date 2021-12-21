using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Base Stat")]
public class BaseStat : ScriptableObject
{
    [Header("Rank Multiplier")]
    public float highRank = 1.5f;
    public float mediumRank = 1f;
    public float lowRank = 0.5f;

    [Header("Base Stat")]
    public float baseHealth = 100f;
    public float baseDamage = 30f;
    public float baseAbility = 20f;
    public float warriorAttackRangeOffest = 0.5f;
    public float archerAttakRange = 15f;
    public float healingRange = 10f;

    [Header("Action Points")]
    public int baseActionPoints = 5;
    public int maxActionPoints = 8;
    public int distancesPerAP = 3;
    public int basicAttackAPCost = 2;



    //角色設定時的預設值,方便測試用
    [Header("Default Stat")]
    public DefaultStat[] defaultStats = new DefaultStat[8];

    [System.Serializable]
    public struct DefaultStat
    {
        public int occupationIndex;
        public int healthIndex;
        public int abilityIndex;
    }
    

}
