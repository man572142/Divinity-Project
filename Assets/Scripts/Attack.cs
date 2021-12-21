using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Occupation myOccupation;
    Team myTeam;
    int myID;
    /// <summary>已在Character裡根據職業計算出結果,只需要再加上Target的body半徑 </summary>
    float attackRange = 0;
    float attackDamage = 0;
    Skill skill;
    int basicAttackAP = 0;
    [SerializeField] LineRenderer attackLine = null;


    private void Awake()
    {
        skill = GetComponent<Skill>();
    }


    public int ToAttack(Character target,bool mouseClick,int currentAP)
    {
        //Debug.Log(gameObject.name + skill.SkillIndex);
        if (skill.SkillIndex > 0 )
        {
            //使用技能
            return skill.ActSkill(target, mouseClick);
            //依技能判定攻擊距離
        }
        else
        {
            if (target.Team == myTeam) return 0;


            //使用普攻
            float targetDistance = Vector3.Distance(transform.position, target.transform.position);

            if (attackRange + target.GetBodySize() >= targetDistance)
            {
                DrawLine(target.transform.position);

                if (mouseClick) //如果有按下滑鼠
                {
                    //執行動畫   
                    target.GetComponent<HPController>().HPChange(attackDamage);

                    GetComponent<Character>().ActionEnd(); //動畫播完,血也扣完
                }

                return basicAttackAP; // 回傳消耗AP  (普通攻擊)
            }

        }

        return 0; //上述動作都沒有執行成功,代表什麼事都不做,回傳消耗0 AP
    }

    public void DrawLine(Vector3 target)
    {
        //顯示路徑線段
        attackLine.positionCount = 2;
        Vector3[] line = new Vector3[2] { Vector3.zero + Vector3.up, attackLine.transform.InverseTransformPoint(target) + Vector3.up };

        attackLine.SetPositions(line);

    }

    public void CleanPathLine()
    {
       attackLine.positionCount = 0;
    }



    #region 初始化

    public void Initialize(Occupation occupation,Team team,int id,float atkRange,int atkAP)
    {
        myOccupation = occupation;
        myTeam = team;
        myID = id;
        attackRange = atkRange;
        attackDamage = GameManager.instance.characterStats[myID].ability;
        basicAttackAP = atkAP;
    }
    #endregion
}