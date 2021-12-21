using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Occupation myOccupation;
    Team myTeam;
    int myID;
    /// <summary>�w�bCharacter�̮ھ�¾�~�p��X���G,�u�ݭn�A�[�WTarget��body�b�| </summary>
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
            //�ϥΧޯ�
            return skill.ActSkill(target, mouseClick);
            //�̧ޯ�P�w�����Z��
        }
        else
        {
            if (target.Team == myTeam) return 0;


            //�ϥδ���
            float targetDistance = Vector3.Distance(transform.position, target.transform.position);

            if (attackRange + target.GetBodySize() >= targetDistance)
            {
                DrawLine(target.transform.position);

                if (mouseClick) //�p�G�����U�ƹ�
                {
                    //����ʵe   
                    target.GetComponent<HPController>().HPChange(attackDamage);

                    GetComponent<Character>().ActionEnd(); //�ʵe����,��]����
                }

                return basicAttackAP; // �^�Ǯ���AP  (���q����)
            }

        }

        return 0; //�W�z�ʧ@���S�����榨�\,�N����Ƴ�����,�^�Ǯ���0 AP
    }

    public void DrawLine(Vector3 target)
    {
        //��ܸ��|�u�q
        attackLine.positionCount = 2;
        Vector3[] line = new Vector3[2] { Vector3.zero + Vector3.up, attackLine.transform.InverseTransformPoint(target) + Vector3.up };

        attackLine.SetPositions(line);

    }

    public void CleanPathLine()
    {
       attackLine.positionCount = 0;
    }



    #region ��l��

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