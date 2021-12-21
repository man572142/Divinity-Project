using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    Occupation myOccupation;
    Team myTeam;
    SkillSet skillSet;
    Character character;
    Attack attack;
    int myID;
    float attackRange = 0;
    float attackDamage = 0;
    /// <summary>0是普攻,1是按鈕1技能,2是按鈕2技能... </summary>
    int skillIndex = 0;
    SkillSet[] skillValue = new SkillSet[3];
    public int SkillIndex { get { return skillIndex; } }  //讓Attack讀取技能狀態

    string skillID;
    float skillDamage;
    float skillRange;
    int skillAP;
    float targetDistance;

    private void Awake()
    {
        attack = GetComponent<Attack>();
    }


    public void SetSkillIndex(int index)
    {
        skillIndex = index;
    }

    public int ActSkill(Character target, bool mouseClick)
    {
        switch (myOccupation)
        {
            case Occupation.Warrior:
                skillID = (10 + skillIndex).ToString();
                skillValue[skillIndex-1] = Resources.Load<SkillSet>("Warrior/Skills/" + skillID);
                skillDamage = attackDamage * skillValue[skillIndex - 1].attackCoefficient;
                skillRange = attackRange * skillValue[skillIndex - 1].skillRangeCoefficient;
                skillAP = skillValue[skillIndex - 1].actionPoint;

                if (target.Team == myTeam) return 0;
                targetDistance = Vector3.Distance(transform.position, target.transform.position);

                if (skillRange + target.GetBodySize() >= targetDistance)
                {
                    attack.DrawLine(target.transform.position);

                    if (mouseClick)
                    {
                        target.GetComponent<HPController>().HPChange(skillDamage);
                        GetComponent<Character>().ActionEnd(); //動畫播完,血也扣完
                    }
                    return skillAP; // 回傳消耗AP  (普通攻擊)
                }
                break;

            case Occupation.Archer:
                skillID = (20 + skillIndex).ToString();
                skillValue[skillIndex - 1] = Resources.Load<SkillSet>("Archer/Skills/" + skillID);
                skillDamage = attackDamage * skillValue[skillIndex - 1].attackCoefficient;
                skillRange = attackRange * skillValue[skillIndex - 1].skillRangeCoefficient;
                skillAP = skillValue[skillIndex - 1].actionPoint;

                if (target.Team == myTeam) return 0;
                targetDistance = Vector3.Distance(transform.position, target.transform.position);

                if (skillRange + target.GetBodySize() >= targetDistance)
                {
                    attack.DrawLine(target.transform.position);

                    if (mouseClick)
                    {
                        target.GetComponent<HPController>().HPChange(skillDamage);
                        GetComponent<Character>().ActionEnd(); //動畫播完,血也扣完
                    }
                    return skillAP; // 回傳消耗AP  (普通攻擊)
                }
                break;
            case Occupation.Healer:
                skillID  = (30 + skillIndex).ToString();
                skillValue[skillIndex - 1] = Resources.Load<SkillSet>("Healer/Skills/" + skillID);
                skillDamage = attackDamage * skillValue[skillIndex - 1].attackCoefficient;
                Debug.Log(attackDamage);
                Debug.Log(skillDamage);
                skillRange = attackRange * skillValue[skillIndex - 1].skillRangeCoefficient;
                skillAP = skillValue[skillIndex - 1].actionPoint;

                if ((skillID == "31" || skillID == "32") && target.Team == myTeam)
                {
                    targetDistance = Vector3.Distance(transform.position, target.transform.position);

                    if (skillRange + target.GetBodySize() >= targetDistance)
                    {
                        attack.DrawLine(target.transform.position);

                        if (mouseClick)
                        {
                            target.GetComponent<HPController>().HPChange(skillDamage * -1);
                            GetComponent<Character>().ActionEnd(); //動畫播完,血也扣完
                        }
                        return skillAP; // 回傳消耗AP  (普通攻擊)
                    }
                }

                if ((skillID == "33" ) && target.Team != myTeam)
                {
                    targetDistance = Vector3.Distance(transform.position, target.transform.position);

                    if (skillRange + target.GetBodySize() >= targetDistance)
                    {
                        attack.DrawLine(target.transform.position);

                        if (mouseClick)
                        {
                            target.GetComponent<HPController>().HPChange(skillDamage);
                            GetComponent<Character>().ActionEnd(); //動畫播完,血也扣完
                        }
                        return skillAP; // 回傳消耗AP  (普通攻擊)
                    }
                }
                break;
        }
        return 0;
    }


    #region 初始化

    public void Initialize(Occupation occupation, Team team, int id, float atkRange)
    {
        myOccupation = occupation;
        myTeam = team;
        myID = id;
        attackRange = atkRange;
        attackDamage = GameManager.instance.characterStats[myID].ability;
    }
    #endregion

}
