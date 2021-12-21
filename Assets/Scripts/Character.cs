using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    [SerializeField] Profile profile = null;
    [SerializeField] Team myTeam = default;
    public Team Team { get { return myTeam; } }
    [SerializeField][Range(1,4)] int characterIndex = 0;
    int characterID = 0;
    [SerializeField] BaseStat stat = null;
    Movement movement;
    Attack attack;
    Skill skill;
    NavMeshAgent navMeshAgent;
    NavMeshObstacle navMeshObstacle;
    [SerializeField] HPController hPController = null;
    [SerializeField] TurnBaseSystem turnBaseSystem = null;
    [SerializeField] Text warningText = null;
    [SerializeField] ActionPointsUI actionPointsUI = null;

    int currentAP = 0;
    int baseAP = 0;
    int maxAP = 0;
    
    
    float attackRange = 0f;
    bool isMyTurn = false;
    bool isActing = false;

    Occupation myOccupation = Occupation.None;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        attack = GetComponent<Attack>();
        skill = GetComponent<Skill>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshObstacle = GetComponent<NavMeshObstacle>();
    }


    public void ActiveCharater(bool activeState)
    {
        navMeshAgent.enabled = activeState;
        navMeshObstacle.enabled = !activeState;
        profile.SetHighlight(activeState);
        hPController.SetMainBarState(activeState);
    
        if(activeState) currentAP = Mathf.Clamp(currentAP + baseAP, 0, maxAP);  //換到此角色的回合,就獲得新的行動點數

        isMyTurn = activeState;
    }

    public void ActionEnd()
    {
        isActing = false;
    }

    private bool InteractWithUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }


    private void Update()
    {
        if (!isMyTurn) return;


        if (currentAP <= 0 && !movement.IsMoving())
        {
            attack.CleanPathLine();
            ActiveCharater(false);
            turnBaseSystem.TurnFinished();
            return;
        }

        if (InteractWithUI())
        {
            movement.CleanPathLine();
            attack.CleanPathLine();
            actionPointsUI.ShowAP(currentAP, 0);
            return;  //如果游標碰的是UI就return
        }


        RaycastTarget();
    }

    private void RaycastTarget()
    {
        if (isActing) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);

        if (hasHit)
        {
            Character target = hit.transform.GetComponent<Character>();
            if (target != null)  //目標是一個角色
            {
                movement.CleanPathLine();

                int estimateCostAP = attack.ToAttack(target, false,currentAP);
                if (currentAP < estimateCostAP)
                {
                    actionPointsUI.ShowAP(currentAP, 0);
                    return;  //如果剩餘AP不夠就return
                }
                else actionPointsUI.ShowAP(currentAP,estimateCostAP );

                if (Input.GetMouseButtonDown(0))
                {
                    if (estimateCostAP <= 0) return;  //判定是否能攻擊成功,若不行就return

                    isActing = true;
                    currentAP -= attack.ToAttack(target,true,currentAP);  //必須在isActing = true後,否則動作完成後本來要isActing = false的卻又被設回true
                }


            }
            else  //目標不是一個角色
            {
                attack.CleanPathLine();

                int estimateCostAP = movement.PredictedPath(hit, currentAP);  //顯示預測路線,同時回傳預計消耗AP
                actionPointsUI.ShowAP(currentAP, estimateCostAP);

                if (Input.GetMouseButtonDown(0))
                {
                    currentAP -= movement.ToMove();  //前往目標並消耗AP
                    isActing = true;
                    actionPointsUI.ShowAP(currentAP, 0);
                }
            }
        }
    }

    private void ShowStat()
    {

        profile.SetOccupation(GameManager.instance.characterStats[characterID].occupation.ToString());
        //profile.SetHealth(GameManager.instance.characterStats[characterID].health);
        //profile.SetAbility(GameManager.instance.characterStats[characterID].ability);
    }

    #region 初始化
    public void InitializeCharacter()
    {
        CheckID();
        ShowStat();
        SetOccupation();
        SetHealth();

        if (attack != null) attack.Initialize(myOccupation, myTeam, characterID,attackRange,stat.basicAttackAPCost);
        if (skill != null) skill.Initialize(myOccupation, myTeam, characterID,attackRange);
        baseAP = stat.baseActionPoints;
        maxAP = stat.maxActionPoints;
        movement.SetMovingCost(stat.distancesPerAP);
    }

    private void SetHealth()
    {
        hPController.SetHP(GameManager.instance.characterStats[characterID].health);
    }

    public float GetBodySize()
    {
        return GetComponent<CapsuleCollider>().radius * transform.localScale.x;
    }

    private void SetOccupation()
    {
        switch(GameManager.instance.characterStats[characterID].occupation)
        {
            case Occupation.Warrior:
                attackRange = GetBodySize() + stat.warriorAttackRangeOffest;
                myOccupation = Occupation.Warrior;
                break;
            case Occupation.Archer:
                attackRange = stat.archerAttakRange;
                myOccupation = Occupation.Archer;
                break;
            case Occupation.Healer:
                attackRange = stat.healingRange;
                myOccupation = Occupation.Healer;
                break;

        }
    }

    private void CheckID()
    {
        if (characterIndex == 0) return;

        if (myTeam == Team.Player1)
        {
            characterID = characterIndex * 1 - 1;
        }
        else if (myTeam == Team.Player2)
        {
            characterID = characterIndex * 2 - 1;
        }
    }

    #endregion

}
